using AutoMapper;
using Minio;
using Minio.DataModel.Args;
using Serilog;
using TrekkingApi.Domain.DTO.Pin;
using TrekkingApi.Domain.Entity;
using TrekkingApi.Domain.Interfaces.Services;
using TrekkingApi.Domain.Result;
using TrekkingApi.Application.Resources;
using TrekkingApi.Domain.Enum;
using TrekkingApi.Domain.Interfaces.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TrekkingApi.Domain.Options.MinioOptions;

namespace TrekkingApi.Application.Services
{
    public class PinService : IPinService
    {
        private readonly IPinUnitOfWork _pinUnitOfWork;
        private readonly IMinioClient _minio;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly string _bucket;
        private readonly string _minioEndpoint;

        public PinService(
            ILogger logger,
            IMapper mapper,
            IMinioClient minio,
            IPinUnitOfWork pinUnitOfWork,
            IOptions<MinioSettings> options)
        {

            _logger = logger;
            _mapper = mapper;
            _minio = minio;
            _pinUnitOfWork = pinUnitOfWork;
            _minioEndpoint = options.Value.Endpoint;
            _bucket = options.Value.BucketName;
        }



        /// <inheritdoc />
        public async Task<BaseResult<GetPinDTO>> GetPinByIdAsync(long pinId)
        {
            //ЗАЛОГИРОВАТЬ И ЗАВАЛИДИРОВАТЬ ВСЁ
            try
            {
                //Все поля pin завалидировать
                var pin = await _pinUnitOfWork.Pins.GetAll()
                    .Where(p => p.Id == pinId)
                    .Include(p => p.Meta)
                    .FirstOrDefaultAsync();

                if (pin == null || pin.Meta == null)
                {
                    return new BaseResult<GetPinDTO>
                    {
                        ErrorMessage = ErrorMessage.PinOrMetaNotFound,
                        ErrorCode = (int)ErrorCodes.PinOrMetaNotFound
                    };
                }

    

                var dto = new GetPinDTO
                {
                    Meta = new GetPinMetaDTO
                    {
                        Title = pin.Title,
                        Description = pin.Description,
                        CollectionId = "UNKNOWN", // заполнить при наличии связи
                        Link = pin.FullUrl,
                        Tags = Array.Empty<string>(), // добавить если будут
                        Location = "NOT_IMPLEMENTED", // если будет таблица LocationEntity
                        FileMeta = new FileInfoDTO
                        {
                            Width = pin.Meta.Width,
                            Height = pin.Meta.Height
                        }
                    }
                };


                return new BaseResult<GetPinDTO> 
                { 
                    Data = dto 
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while retrieving pin {PinId}", pinId);
                return new BaseResult<GetPinDTO>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
        }







        public async Task<BaseResult<long>> CreatePin(PinRequestDTO pinRequest, string username)
        {
            //ЗАЛОГИРОВАТЬ И ЗАВАЛИДИРОВАТЬ ВСЁ



            // 1) подготавливаем имя файла в MinIO
            var ext = Path.GetExtension(pinRequest.Meta.FileMeta.RawFilename);
            var objectName = $"{Guid.NewGuid()}{ext}";
            var fileUrl = $"storage.fasberry.su/{_bucket}/{objectName}";

            //// Проверка наличия бакета
            //bool found = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucket));
            //if (!found)
            //{
            //    _logger.LogInformation("Bucket {BucketName} does not exist, creating...", _bucket);
            //    await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket).WithRegion("us-east-1")); // Укажите нужный регион
            //    _logger.LogInformation("Bucket {BucketName} created successfully.", _bucket);
            //}


            // 2) загружаем в MinIO
            try
            {
                using var ms = new MemoryStream(pinRequest.File);
                var test =  await _minio.PutObjectAsync(new PutObjectArgs()
                   .WithBucket(_bucket)
                   .WithObject(objectName)
                   .WithStreamData(ms)
                   .WithObjectSize(ms.Length)
                   .WithContentType(pinRequest.Meta.FileMeta.Type)
                 );

            }
            catch (Exception uploadEx)
            {
                // не смогли залить файл — просто возвращаем 500
                return new BaseResult<long>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
            long pinId = 0;

            // 3) сохраняем метаданные в базе внутри транзакции EF
            using var tx = await _pinUnitOfWork.BeginTransactionAsync();
            try
            {
                var user = await _pinUnitOfWork.Users.GetAll()
                    .FirstOrDefaultAsync(x => x.Login == username); 
                if (user == null)
                {
                    tx.Rollback();
                }

                var pin = new PinEntity
                {
                    OwnerId = user.Id,
                    Title = pinRequest.Meta.Title,
                    Description = pinRequest.Meta.Description,
                    FullUrl = fileUrl,
                    ThumbnailUrl = fileUrl,       // или генерировать thumbnail
                    Category = "ZAGLUSHKACATEGORY",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _pinUnitOfWork.Pins.CreateAsync(pin);
                await _pinUnitOfWork.SaveChangesAsync();

                // доп. таблица pins_meta
                var meta = new PinMetaEntity
                {
                    PinId = pin.Id,
                    Width = pinRequest.Meta.FileMeta.Width,
                    Height = pinRequest.Meta.FileMeta.Height,
                    Size = pinRequest.File.Length,
                    // location → сохраняем как строку или FK в отдельную таблицу locations
                };
                await _pinUnitOfWork.PinsMeta.CreateAsync(meta);
                await _pinUnitOfWork.SaveChangesAsync();
                pinId = pin.Id;
                await tx.CommitAsync();
            }
            catch (Exception dbEx)
            {
                pinId = 0;
                // 4) если в БД что‑то сломалось — нужно удалить уже загруженный файл
                try
                {
                    await _minio.RemoveObjectAsync(new RemoveObjectArgs()
                      .WithBucket(_bucket)
                      .WithObject(objectName)
                    );
                }
                catch
                {
                    // здесь можно логировать неудачный компенсационный удалитель
                }

                return new BaseResult<long>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }

            if (pinId == 0)
            {
                return new BaseResult<long>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }
            else
            {
                return new BaseResult<long>
                {
                    Data = pinId
                };
            }

        }
    }
}
