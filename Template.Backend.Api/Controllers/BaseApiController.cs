using Template.Backend.Service;
using Template.Backend.Service.Audit;
using Microsoft.AspNetCore.Mvc;
using Template.Backend.Model.Exceptions;
using Template.Backend.Service.Validation;
using Template.Backend.Model;
using Template.Backend.Model.Audit;
using AutoMapper;
using System.Globalization;
using Template.Backend.Api.Utilities;

namespace Template.Backend.Api.Controllers
{
    public abstract class BaseApiController<Entity, AuditEntity> : ControllerBase where Entity : IEntity where AuditEntity : IAuditEntity
    {
        protected const int _detailsDepth = 2;
        protected const int _getAllDepth = 1;
        /// <summary>
        /// The date format for Snapshot method
        /// </summary>
        protected const string _dateFormat = "yyyyMMddTHHmmss";

        /// <summary>
        /// The media type
        /// </summary>
        protected const string _mediaType = "application/json";
        private readonly IService<Entity> _Service;
        private readonly IServiceAudit<AuditEntity> _AuditService;
        protected readonly IMapper _mapper;
        protected readonly ILogger<Entity> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController{Entity, AuditEntity}" /> class.
        /// </summary>
        /// <param name="Service">The service.</param>
        /// <param name="AuditService">The audit service.</param>
        public BaseApiController(IService<Entity> Service, IServiceAudit<AuditEntity> AuditService, IMapper mapper, ILogger<Entity> logger)
        {
            _Service = Service;
            _AuditService = AuditService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// entity Count.
        /// </summary>
        /// <returns>Entity</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult Count()
        {
            _logger.LogInformation($"Count for {typeof(Entity).Name} entity");
            int count = _Service.Count();
            return Ok(count);
        }

        /// <summary>
        /// Get All.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        protected virtual IActionResult Get()
        {
            _logger.LogInformation($"GetAll for {typeof(Entity).Name} entity");
            var entities = _Service.GetAll();
            if (entities != null && entities.Any())
                return Ok(entities);
            else
            {
                throw new IdNotFoundException($"No element found for {typeof(Entity).Name} entity");
            }
        }

        /// <summary>
        /// Gets Entity by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Entity</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        protected virtual IActionResult Get(int id)
        {
            _logger.LogInformation($"Search for {typeof(Entity).Name} with Id {id}");
            var entity = _Service.GetById(id);
            if (entity != null)
                return Ok(entity);
            else
            {
                throw new IdNotFoundException($"No element found for this id {id}");
            }
        }

        /// <summary>
        /// Insert the specified Entity.
        /// </summary>
        /// <param name="entity">The Entity to Insert.</param>
        /// <returns>Created ressource with statut code (Created if is Inserted otherwise error message)</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult Post([FromBody] Entity entity)
        {
            _logger.LogInformation($"Post {typeof(Entity).Name}");
            _Service.Add(entity);

            // model state test
            if (!_Service.GetValidationDictionary().IsValid() || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_Service.GetValidationDictionary());
                throw new ModelStateException("One or more validation errors occurred.", _Service.GetValidationDictionary().ToReadOnlyDictionary());
            }

            _Service.Save();
            var actionName = nameof(Get);
            var routeValues = new { id = entity.ID };
            return CreatedAtAction(actionName, routeValues, entity);
        }

        /// <summary>
        /// Insert the specified Entities.
        /// </summary>
        /// <param name="entities">The Entities to Insert.</param>
        /// <returns>Created ressource with statut code (Created if is Inserted otherwise error message)</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult PostList([FromBody] IEnumerable<Entity> entities)
        {
            _logger.LogInformation($"PostList {typeof(Entity).Name}");
            _Service.AddRange(entities);
            _Service.Save();
            var actionName = nameof(Get);
            return CreatedAtAction(actionName, entities);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult Put(int id, [FromBody] Entity entity)
        {
            _logger.LogInformation($"Put {typeof(Entity).Name} with Id {id}");
            if (entity != null)
            {
                if (!_Service.CheckIfExist(e => e.ID == id))
                {
                    throw new IdNotFoundException($"No element found for this id {id} for {typeof(Entity)?.Name}");
                }
                entity.ID = id;
                _Service.Update(entity);

                // model state test
                if (!_Service.GetValidationDictionary().IsValid() || !ModelState.IsValid)
                {
                    AddServiceErrorsToModelState(_Service.GetValidationDictionary());
                    throw new ModelStateException("One or more validation errors occurred.", _Service.GetValidationDictionary().ToReadOnlyDictionary());
                }

                _Service.Save();

                return Ok();
            }
            else
            {
                throw new BadRequestException($"No values defined in request body for {typeof(Entity)?.Name}");
            }
        }

        /// <summary>
        /// Gets Audit of the specified ID.
        /// </summary>
        /// <param name="id">The audit Id.</param>
        /// <returns>Audit line</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult AuditId(int id)
        {
            _logger.LogInformation($"Search for {typeof(AuditEntity).Name} with Id {id}");
            AuditEntity auditEntity = _AuditService.GetById(id);

            if (auditEntity != null)
                return Ok(auditEntity);
            else
            {
                throw new IdNotFoundException($"No element found for this id {id}");
            }
        }

        /// <summary>
        /// Deletes Entity with the specified Id.
        /// </summary>
        /// <param name="id">ID to Delete</param>
        /// <returns>Http Response with statut code (NoContent (204) if is Deleted otherwise error message)</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult Delete(int id)
        {
            _logger.LogInformation($"Delete {typeof(Entity).Name} with Id {id}");
            if (id > 0)
            {
                _Service.Delete(id);
                _Service.Save();
                return NoContent();
            }
            throw new CanNotBeDeletedException($"Not a valid Id {id} for {typeof(Entity)?.Name}");
        }

        /// <summary>
        /// Gets Audit List of company with the specified Id.
        /// its provide All operations performed on this company
        /// </summary>
        /// <param name="id">The Company ID.</param>
        /// <returns>List of company Audits</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult Audit(int id)
        {
            _logger.LogInformation($"Audit {typeof(Entity).Name} with Id {id}");
            IEnumerable<AuditEntity> auditEntity = _AuditService.GetAuditById(id);

            if (auditEntity != null && auditEntity.Any())
            {
                return Ok(auditEntity);
            }
            else
            {
                throw new NoElementFoundException($"No element found for this id {id} for {typeof(AuditEntity)?.Name}");
            }
        }

        /// <summary>
        /// Gets the paged list of Entities.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Entities</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult GetPagedList(int pageNo, int pageSize)
        {
            _logger.LogInformation($"GetPagedList {typeof(Entity).Name} with pageNo {pageNo} and pageSize {pageSize}");
            int count = _Service.Count();
            IEnumerable<Entity> list = _Service.GetPagedList(pageNo, pageSize);

            if (list != null && list.Any())
            {
                AddCountHeaders(list, count);
                return Ok(list);
            }  
            else
            {
                throw new NoElementFoundException($"No element found for {typeof(Entity).Name} entity");
            }
        }

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>List of Objects as Json response</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult GetSnapshot(string dateTime)
        {
            try
            {
                _logger.LogInformation($"GetSnapshot {typeof(Entity).Name} with dateTime {dateTime}");

                DateTime dateParsed = DateTime.ParseExact(dateTime, _dateFormat, CultureInfo.InvariantCulture,
                                                    DateTimeStyles.AdjustToUniversal);

                IEnumerable<AuditEntity> entityAuditList = _AuditService.GetAllSnapshot(dateParsed);

                if (entityAuditList != null && entityAuditList.Any())
                {
                    IEnumerable<Entity> entityList = _mapper.Map<IEnumerable<AuditEntity>,
                                                IEnumerable<Entity>>(entityAuditList);
                    return Ok(entityAuditList);
                }
                else
                {
                    throw new NoElementFoundException($"No element found for this dateTime {dateTime} for {typeof(AuditEntity)?.Name}");
                }
            }
            catch (FormatException)
            {
                throw new DateTimeFormatException($"DateTime parameter format {_dateFormat}");
            }
        }

        /// <summary>
        /// Gets the snapshot.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Object as Json response
        /// </returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public virtual IActionResult GetSnapshot(string dateTime, int id)
        {
            try
            {
                _logger.LogInformation($"GetSnapshot {typeof(Entity).Name} with dateTime {dateTime} and Id {id}");
                DateTime dateParsed = DateTime.ParseExact(dateTime, _dateFormat, CultureInfo.InvariantCulture,
                                                    DateTimeStyles.AdjustToUniversal);

                AuditEntity auditEntity = _AuditService.GetByIdSnapshot(dateParsed, id);

                if (auditEntity != null)
                {
                    Entity entity = _mapper.Map<AuditEntity, Entity>(auditEntity);
                    return Ok(entity);
                }
                else
                {
                    throw new IdNotFoundException($"No Snapshot found for this {id} on {dateTime}");
                }
            }
            catch (FormatException)
            {
                throw new DateTimeFormatException($"DateTime parameter format {_dateFormat}");
            }
        }

        /// <summary>
        /// Add service Validation errors to Api controller model state
        /// </summary>
        /// <param name="validationDictionary">Validation Dictionary class</param>
        protected void AddServiceErrorsToModelState(IValidationDictionary validationDictionary)
        {
            var errors = validationDictionary.ToDictionary();
            if (errors == null)
            {
                return;
            }

            foreach (var error in errors)
            {
                foreach (var entry in error.Value.AsEnumerable())
                {
                    ModelState.AddModelError(error.Key, entry);
                }
            }
        }

        protected void AddCountHeaders(IEnumerable<Entity> list, int count)
        {
            HttpContext?.Response?.Headers?.Add(Constants.CustomHeader_total_count_found, count.ToString());
            HttpContext?.Response?.Headers?.Add(Constants.CustomHeader_total_count_returned, list?.Count().ToString());
        }
    }
}