using AutoMapper;
using Template.Backend.Api.Exceptions;
using Template.Backend.Api.Helpers;
using Template.Backend.Service;
using Template.Backend.Service.Audit;
using Template.Backend.Service.Validation;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Template.Backend.Model.Exceptions;
using Template.Backend.Model.Audit;
using Template.Backend.Model;

namespace Template.Backend.Api.Controllers
{
    public abstract class BaseApiController<Entity, AuditEntity> : ApiController where Entity : class where AuditEntity : class
    {
        protected const int _detailsDepth = 2;
        protected const int _getAllDepth = 1;
        /// <summary>
        /// The date format for Snapshot method
        /// </summary>
        protected const string _dateFormat = "yyyyMMddTHHmmss";
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The media type
        /// </summary>
        protected const string _mediaType = "application/json";
        private readonly IService<Entity> _Service;
        private readonly IServiceAudit<AuditEntity> _AuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController{Entity, AuditEntity}" /> class.
        /// </summary>
        /// <param name="Service">The service.</param>
        /// <param name="AuditService">The audit service.</param>
        public BaseApiController(IService<Entity> Service, IServiceAudit<AuditEntity> AuditService)
        {
            _Service = Service;
            _AuditService = AuditService;
        }

        /// <summary>
        /// entity Count.
        /// </summary>
        /// <returns>Entity</returns>
        public virtual IHttpActionResult Count()
        {
            int count = _Service.Count();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, count);
            return ResponseMessage(response);
        }

        /// <summary>
        /// Gets All
        /// </summary>
        /// <returns>List</returns>
        public virtual IHttpActionResult Get()
        {
            int count = _Service.Count();
            IEnumerable<Entity> list = _Service.GetAll();
            if (list != null && list.Any())
            {
                return ResponseMessage(ToJsonResponse(list, _getAllDepth, count));
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found"), Request));
            }
        }

        /// <summary>
        /// Gets Entity by Id.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Entity</returns>
        public virtual IHttpActionResult Get(int id)
        {
            var entity = _Service.GetById(id);
            if (entity != null)
                return ResponseMessage(ToJsonResponse(entity, HttpStatusCode.OK, _detailsDepth));
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new IdNotFoundException("No element found for this id"), Request));
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">Entity ID .</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>Entity</returns>
        public virtual IHttpActionResult Get(int id, int depth)
        {
            var list = _Service.GetById(id);
            if (list != null)
                return ResponseMessage(ToJsonResponse(list, HttpStatusCode.OK, depth));
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new IdNotFoundException("No element found for this id"), Request));
            }
        }

        /// <summary>
        /// Gets the paged list of Entities.
        /// </summary>
        /// <param name="pageNo">page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Entities</returns>
        public virtual IHttpActionResult GetPagedList(int pageNo, int pageSize)
        {
            int count = _Service.Count();
            IEnumerable<Entity> list = _Service.GetPagedList(pageNo, pageSize);
            if (list != null && list.Any())
            {
                return ResponseMessage(ToJsonResponse(list, _detailsDepth, count));
            }
            else
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found"), Request));
            }
        }

        /// <summary>
        /// Deletes Entity with the specified Id.
        /// </summary>
        /// <param name="id">ID to Delete</param>
        /// <returns>Http Response with statut code (NoContent (204) if is Deleted otherwise error message)</returns>
        public virtual IHttpActionResult Delete(int id)
        {
            if (id > 0)
            {
                _Service.Delete(id);
                _Service.Save(User.Identity.Name);
                return new StatusCodeResult(HttpStatusCode.NoContent, Request);
            }
            return ResponseMessage(ApiExceptionResponse.Throw(new BusinessException("Not a valid Id"), Request));
        }

        /// <summary>
        /// Insert the specified Entity.
        /// </summary>
        /// <param name="entity">The Entity to Insert.</param>
        /// <returns>Created ressource with statut code (Created if is Inserted otherwise error message)</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IHttpActionResult Post([FromBody] Entity entity)
        {
            _Service.Add(entity);

            // model state test
            if (!_Service.GetValidationDictionary().IsValid() || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_Service.GetValidationDictionary());
                return ResponseMessage(InvalidModelStateToJsonResponse(_Service.GetValidationDictionary().ToDictionary()));
            }

            _Service.Save(User.Identity.Name);
            return ResponseMessage(ToJsonResponse(entity, HttpStatusCode.Created));
        }

        /// <summary>
        /// Insert the specified Entity List.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        /// <returns>
        /// Created ressource with statut code (Created if is Inserted otherwise error message)
        /// </returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IHttpActionResult PostList([FromBody] IEnumerable<Entity> entityList)
        {
            _Service.AddRange(entityList);

            // model state test
            if (!_Service.GetValidationDictionary().IsValid() || !ModelState.IsValid)
            {
                AddServiceErrorsToModelState(_Service.GetValidationDictionary());
                return ResponseMessage(InvalidModelStateToJsonResponse(_Service.GetValidationDictionary().ToDictionary()));
            }

            _Service.Save(User.Identity.Name);
            return ResponseMessage(ToJsonResponse(entityList, HttpStatusCode.Created));
        }

        /// <summary>
        /// Update the Entity with the specified ID.
        /// </summary>
        /// <param name="id">the Entity ID to Update.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Http Response with statut code (OK if is Updated otherwise error message)
        /// </returns>
        /// <exception cref="BadRequestException">No values defined in request body</exception>
        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual IHttpActionResult Put(int id, Entity entity)
        {
            if (entity != null)
            {
                IEntity iEntity = (IEntity)entity;
                iEntity.ID = id;
                Entity entityToUpdate = (Entity)iEntity;
                _Service.Update(entityToUpdate);

                // model state test
                if (!_Service.GetValidationDictionary().IsValid() || !ModelState.IsValid)
                {
                    HttpResponseMessage responseMsg = InvalidModelStateToJsonResponse(_Service.GetValidationDictionary().ToDictionary());
                    return ResponseMessage(responseMsg);
                }

                _Service.Save(User.Identity.Name);

                return Ok();
            }
            else
            {
                throw new BadRequestException("No values defined in request body");
            }
        }

        /// <summary>
        /// Gets Audit of the specified ID.
        /// </summary>
        /// <param name="id">The audit Id.</param>
        /// <returns>Audit line</returns>
        public virtual IHttpActionResult AuditId(int id)
        {
            AuditEntity auditEntity = _AuditService.GetById(id);

            if (auditEntity != null)
                return ResponseMessage(AuditToJsonResponse((IAuditEntity)auditEntity, 2));

            return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("Audit not found"), Request));
        }

        /// <summary>
        /// Return Json response from List of Entity object.
        /// </summary>
        /// <param name="obj">List of Entities</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="totalCountFound">The total count found.</param>
        /// <returns>
        /// Json response
        /// </returns>
        protected HttpResponseMessage ToJsonResponse(IEnumerable<Entity> obj, HttpStatusCode httpStatusCode, int depth = 2, int totalCountFound = 0)
        {
            int objCount = obj.Count();
            if (totalCountFound < objCount)
                totalCountFound = objCount;
            HttpResponseMessage response = Request.CreateResponse(httpStatusCode);
            string content = ApiHelper.SerializeObjectDepth(obj, depth);
            response.Content = new StringContent(content, Encoding.UTF8, _mediaType);
            response.Headers.Add("X-Total-Count-Found", totalCountFound.ToString());
            response.Headers.Add("X-Total-Count-Returned", objCount.ToString());
            return response;
        }

        /// <summary>
        /// Return Json response from List of Entity object.
        /// </summary>
        /// <param name="obj">List of Entities</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <param name="totalCountFound">The total count found.</param>
        /// <returns>
        /// Json response
        /// </returns>
        protected HttpResponseMessage ToJsonResponse(IEnumerable<Entity> obj, int depth = 1, int totalCountFound = 0)
        {
            int objCount = obj.Count();
            if (totalCountFound < objCount)
                totalCountFound = objCount;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            string content = ApiHelper.SerializeObjectDepth(obj, depth);
            response.Content = new StringContent(content, Encoding.UTF8, _mediaType);
            response.Headers.Add("X-Total-Count-Found", totalCountFound.ToString());
            response.Headers.Add("X-Total-Count-Returned", objCount.ToString());
            return response;
        }

        /// <summary>
        /// Return Json response from Entity object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>
        /// Json response
        /// </returns>
        protected HttpResponseMessage ToJsonResponse(Entity obj, HttpStatusCode httpStatusCode, int depth = 1)
        {
            HttpResponseMessage response = Request.CreateResponse(httpStatusCode);
            string content = ApiHelper.SerializeObjectDepth(obj, depth);
            response.Content = new StringContent(content, Encoding.UTF8, _mediaType);
            return response;
        }

        /// <summary>
        /// Return Json response from Audit Entity object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>Json response</returns>
        protected HttpResponseMessage AuditToJsonResponse(IAuditEntity obj, int depth = 1)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            string content = ApiHelper.SerializeObjectDepth(obj, depth);
            response.Content = new StringContent(content, Encoding.UTF8, _mediaType);
            return response;
        }

        /// <summary>
        /// Return Json response from List of Audit Entities object.
        /// </summary>
        /// <param name="obj">Object to transforme</param>
        /// <param name="depth">The maximum level to achieve for navigation properties serialization.</param>
        /// <returns>Json response</returns>
        protected HttpResponseMessage AuditToJsonResponse(IEnumerable<IAuditEntity> obj, int depth = 1)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            string content = ApiHelper.SerializeObjectDepth(obj, depth);
            response.Content = new StringContent(content, Encoding.UTF8, _mediaType);
            return response;
        }

        /// <summary>
        ///  model state to json response.
        /// </summary>
        /// <param name="obj">Errors Dictionary</param>
        /// <returns>Http Response that contains Errors Dictionary</returns>
        protected HttpResponseMessage InvalidModelStateToJsonResponse(Dictionary<string, IList<string>> obj)
        {
            HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid models state");
            string content = ApiHelper.SerializeObjectDepth(obj, 3);
            response.Content = new StringContent(content, Encoding.UTF8, _mediaType);
            response.ReasonPhrase = "ModelStateException";
            return response;
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

        /// <summary>
        /// Gets the snapshot at the specified date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>List of Objects as Json response</returns>
        public virtual IHttpActionResult GetSnapshot(string dateTime)
        {
            try
            {
                IEnumerable<AuditEntity> entityAuditList;
                IEnumerable<Entity> entityList;
                DateTime dateParsed = DateTime.ParseExact(dateTime, _dateFormat, CultureInfo.InvariantCulture,
                                                    DateTimeStyles.AdjustToUniversal);

                entityAuditList = _AuditService.GetAllSnapshot(dateParsed);

                if (entityAuditList != null && entityAuditList.Any())
                {
                    entityList = Mapper.Map<IEnumerable<AuditEntity>,
                                                IEnumerable<Entity>>(entityAuditList);
                    return ResponseMessage(ToJsonResponse(entityList, 2));
                }
                else
                {
                    return ResponseMessage(ApiExceptionResponse.Throw(new NoElementFoundException("No element found"), Request));
                }
            }
            catch (FormatException)
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new DateTimeFormatException("DateTime parameter format yyyyMMddThhmmss"), Request));
            }
            catch (Exception)
            {
                throw;
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
        public virtual IHttpActionResult GetSnapshot(string dateTime, int id)
        {
            try
            {
                AuditEntity auditEntity;
                Entity entity;
                DateTime dateParsed = DateTime.ParseExact(dateTime, _dateFormat, CultureInfo.InvariantCulture,
                                                    DateTimeStyles.AdjustToUniversal);

                auditEntity = _AuditService.GetByIdSnapshot(dateParsed, id);

                if (auditEntity != null)
                {
                    entity = Mapper.Map<AuditEntity, Entity>(auditEntity);
                    return ResponseMessage(ToJsonResponse(entity, HttpStatusCode.OK));
                }
                else
                {
                    return ResponseMessage(ApiExceptionResponse.Throw(new IdNotFoundException("No Snapshot found for this id"), Request));
                }
            }
            catch (FormatException)
            {
                return ResponseMessage(ApiExceptionResponse.Throw(new DateTimeFormatException("DateTime parameter format yyyyMMddThhmmss"), Request));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}