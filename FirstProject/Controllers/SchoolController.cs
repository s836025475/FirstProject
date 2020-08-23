using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using FirstProject.ViewModels.SchoolVMs;

namespace FirstProject.Controllers
{
    
    [ActionDescription("学校管理")]
    public partial class SchoolController : BaseController
    {
        #region Search
        [ActionDescription("Search")]
        public ActionResult Index()
        {
            var vm = CreateVM<SchoolListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Search")]
        [HttpPost]
        public string Search(SchoolListVM vm)
        {
            if (ModelState.IsValid)
            {
                return vm.GetJson(false);
            }
            else
            {
                return vm.GetError();
            }
        }

        #endregion

        #region Create
        [ActionDescription("Create")]
        public ActionResult Create()
        {
            var vm = CreateVM<SchoolVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Create")]
        public ActionResult Create(SchoolVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }
        #endregion

        #region Edit
        [ActionDescription("Edit")]
        public ActionResult Edit(string id)
        {
            var vm = CreateVM<SchoolVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Edit")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(SchoolVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
                }
            }
        }
        #endregion

        #region Delete
        [ActionDescription("Delete")]
        public ActionResult Delete(string id)
        {
            var vm = CreateVM<SchoolVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("Delete")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = CreateVM<SchoolVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region Details
        [ActionDescription("Details")]
        public ActionResult Details(string id)
        {
            var vm = CreateVM<SchoolVM>(id);
            return PartialView(vm);
        }
        #endregion

        #region BatchEdit
        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = CreateVM<SchoolBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchEdit")]
        public ActionResult DoBatchEdit(SchoolBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit",vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer?["OprationSuccess"]);
            }
        }
        #endregion

        #region BatchDelete
        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = CreateVM<SchoolBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult DoBatchDelete(SchoolBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete",vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer?["OprationSuccess"]);
            }
        }
        #endregion

        #region Import
		[ActionDescription("Import")]
        public ActionResult Import()
        {
            var vm = CreateVM<SchoolImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Import")]
        public ActionResult Import(SchoolImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(WalkingTec.Mvvm.Core.Program._localizer["ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }
        #endregion

        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(SchoolListVM vm)
        {
            return vm.GetExportData();
        }

        [HttpGet]
        [AllRights]
        public string SellGameCommodity()
        {
            var id = Guid.Parse(Request.Query["ID"]);
            var price = Convert.ToDecimal(Request.Query["price"]);
            using (var db = new DataContext())
            {
                var data = db.Schools.Find(id);
                try
                {
                    //if (!string.IsNullOrEmpty(data.SchoolName))
                    //{
                    //    var result = "{ \"obj\" : -1,\"message\": \"该商品暂无法销售\"}";
                    //    return result;

                    //}

                    data.SchoolName = "已出售";
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var result = "{ \"obj\" : -1,\"message\": \"出售失败:" + ex.Message + "\"}";
                    return result;
                }
            }
            //成功
            return "{ \"obj\" : 0}";
        }

    }
}
