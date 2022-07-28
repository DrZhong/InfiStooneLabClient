using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows;
using InfiStoone.LabClient.Runtime;
using InfiStoone.LabClient.Runtime.Entity;
using Newtonsoft.Json;

namespace InfiStoone.LabClient.Services
{
    public class DataService
    {
        //private const string Host = "http://localhost:21021";
        private const string Host = "http://lab-api.infistoone.com";

        private readonly HttpClient _httpClient;

        public DataService()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();  
            }   
              
        }

        public static List<UserFingerDto> CachedUserFinger { get; set; } = new List<UserFingerDto>();

        public int GetMaxFingerId()
        {
            if (!CachedUserFinger.Any()) return 1;
            return CachedUserFinger.Max(w => w.Id) + 1;
        }

        public List<UserFingerDto> GetAllUserFinger()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetAllUserFinger").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<List<UserFingerDto>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {

                    CachedUserFinger = ajaxResult.Result;
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }

        }


        public void AddUserFinger(UserFingerDto input)
        {
            string p = JsonConvert.SerializeObject(input);
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/AddUserFinger", new StringContent(p, Encoding.UTF8, "application/json")).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = JsonConvert.DeserializeObject<AjaxResult<string>>(repose);
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                    
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }

        }

        public PagedResultDto<UserDto> GetUser(string filter, int pageIndex, int pageSize)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/User/GetAll?&SkipCount={(pageIndex-1)*pageSize}&MaxResultCount={pageSize}&IsActive=true&Keyword={filter}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<PagedResultDto<UserDto>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                   
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }

        public AbpUser AccountLogin(string userNameOrEmailAddress, string password)
        {
            string p = JsonConvert.SerializeObject(new
            {
                userNameOrEmailAddress,
                password ,
                rememberClient=true
            });
            var request= _httpClient.PostAsync($"{Host}/api/TokenAuth/Authenticate", new StringContent(p,Encoding.UTF8,"application/json")).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = JsonConvert.DeserializeObject<AjaxResult<AbpUser>>(repose);
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                    Session.User = ajaxResult.Result;
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message,ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }
               
            }
           

        }


        public AbpUser FingerLogin( string code)
        {
            string p = JsonConvert.SerializeObject(new
            {
                code
            });
            var request = _httpClient.PostAsync($"{Host}/api/TokenAuth/LoginByFinger", new StringContent(p, Encoding.UTF8, "application/json")).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = JsonConvert.DeserializeObject<AjaxResult<AbpUser>>(repose);
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                    Session.User = ajaxResult.Result;
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }


        }

        private UserFriendException ThrowAbpError(AjaxResult ajaxResult,string errmsg=null)
        {
            if (ajaxResult.Abp)
            {
                //如果时abp
                return new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
            }
            else
            {
                return new UserFriendException(errmsg);
            }
        }

        /// <summary>
        /// 获取所有仓库
        /// </summary>
        /// <returns></returns>
        public List<WareHouseDto> GetWareHouse()
        {
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Common/GetAllActiveWareHouse?isActive=true").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<List<WareHouseDto>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                { 
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }

        /// <summary>
        /// 获取当前登陆用户拥有的权限
        /// </summary>
        /// <returns></returns>
        public List<WarehousePermissionDto> GetWarehousePermission()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetUserWarehousePermission?wareHouseId={Session.SelectedWareHouse.Id}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<List<WarehousePermissionDto>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                    Session.CurrentPermission = ajaxResult.Result;
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }


        /// <summary>
        /// 获取当前登陆用户拥有的权限
        /// </summary>
        /// <returns></returns>
        public List<ClientStockDto> GetClientStockDto(string filter,int pageSize=20)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/SearchStock?filter={filter}&warehouseId={Session.SelectedWareHouse.Id}&pageSize={pageSize}",new StringContent("")).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<List<ClientStockDto>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                { 
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }


        /// <summary>
        /// 根据code查库存
        /// </summary>
        /// <returns></returns>
        public List<ClientStockDto> GetClientStockByCodeDto(string code, int pageSize = 20)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/SearchStockByCode?code={code}&warehouseId={Session.SelectedWareHouse.Id}&pageSize={pageSize}", new StringContent("")).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<List<ClientStockDto>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }

        /// <summary>
        /// 根据code搜索试剂详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendException"></exception>
        public ReagentStockDto GetReagentStockbyCode(string code)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetReagentDetail?barCode={code}&warehouseId={Session.SelectedWareHouse.Id}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<ReagentStockDto>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                  
                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }


        /// <summary>
        /// 根据code搜索待归还试剂详情
        /// 归还只能是专管试剂
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendException"></exception>
        public ReagentStockDto GetReagentStockBackDetailbyCode(string code)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetMasterStockBackDetail?barCode={code}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<ReagentStockDto>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {

                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }


        /// <summary>
        /// 专管试剂入库
        /// </summary>
        /// <returns></returns>
        public void MasterStockIn(int reagentStockId,decimal weight)
        {
            string p = JsonConvert.SerializeObject(new
            {
                reagentStockId,
                weight,
                warehouseId = Session.SelectedWareHouse.Id
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/MasterStockIn", p.StringToStringContent()).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<string>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }

        }


        public void NormalStockIn(int reagentStockId,int acount)
        {
            string p = JsonConvert.SerializeObject(new
            {
                reagentStockId,
                acount,
                warehouseId = Session.SelectedWareHouse.Id
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/NormalStockIn", p.StringToStringContent()).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<string>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }

        }


        /// <summary>
        /// 试剂出库
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="num"></param>
        /// <exception cref="UserFriendException"></exception>
        public void MasterStockout(string barCode, int num,decimal weight)
        {
            string p = JsonConvert.SerializeObject(new
            {
                barCode,
                num,
                weight,
                warehouseId = Session.SelectedWareHouse.Id
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/StockOut", p.StringToStringContent()).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<string>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }



        /// <summary>
        /// 试剂出库
        /// </summary>
        /// <param name="reagentStockIds"></param> 
        /// <exception cref="UserFriendException"></exception>
        public void MasterStocBack(List<int> reagentStockIds )
        {
            string p = JsonConvert.SerializeObject(new
            {
                reagentStockIds, 
                warehouseId = Session.SelectedWareHouse.Id
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/MasterStockBack", p.StringToStringContent()).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<string>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }


        /// <summary>
        /// 试剂归还
        /// </summary>
        /// <param name="reagentStockIds"></param> 
        /// <exception cref="UserFriendException"></exception>
        public void MasterStocBackV2(int reagentStockId,decimal weight)
        {
            string p = JsonConvert.SerializeObject(new
            {
                reagentStockId,
                weight,
                warehouseId = Session.SelectedWareHouse.Id
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/MasterStockBackV2", p.StringToStringContent()).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<string>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }


        public List<OutOrder> GetMyOrder()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetMyOrder?warehouseId={ Session.SelectedWareHouse.Id}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<List<OutOrder>>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {

                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }

        /// <summary>
        /// 通过订单id获取订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendException"></exception>
        public OutOrder GetOrderById(string orderId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetOrderByOrderId?orderId={ orderId}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<OutOrder>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {

                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }

        /// <summary>
        /// 订单出库
        /// </summary>
        /// <param name="orderId"></param>
        /// <exception cref="UserFriendException"></exception>
        public void OrderStockOut(int orderId,List<OutOrderStockOutInputItem> items=null)
        {
            string p = JsonConvert.SerializeObject(new
            {
                orderId, 
                warehouseId = Session.SelectedWareHouse.Id,
                items= items
            });
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.PostAsync($"{Host}/api/services/app/Client/OutOrderStockOut", p.StringToStringContent()).Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<string>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }

        /// <summary>
        /// 根据订单项获取订单详情
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendException"></exception>
        public NormalReagentStockListDto GetNormalReagentStockByCode(string barCode)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.User.AccessToken); //= $"Bearer {PharmacyWcsConst.AccessToken}";
            var request = _httpClient.GetAsync($"{Host}/api/services/app/Client/GetNormalByCode?barCode={ barCode}").Result;
            var repose = request.Content.ReadAsStringAsync().Result;
            var ajaxResult = repose.StringToAjaxResult<NormalReagentStockListDto>();
            if (ajaxResult == null)
            {
                throw new UserFriendException(repose);
            }
            if (request.StatusCode == HttpStatusCode.OK)
            {
                //请求成功 
                if (ajaxResult.Success)
                {

                    return ajaxResult.Result;
                }
                else
                {
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }

            }
            else
            {
                if (ajaxResult.Abp)
                {
                    //如果时abp
                    throw new UserFriendException(ajaxResult.Error.Message, ajaxResult.Error.Details);
                }
                else
                {
                    throw new UserFriendException(repose);
                }

            }
        }
    }
}