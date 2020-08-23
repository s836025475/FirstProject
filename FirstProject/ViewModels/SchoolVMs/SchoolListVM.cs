using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FirstProject.Models;


namespace FirstProject.ViewModels.SchoolVMs
{
    public partial class SchoolListVM : BasePagedListVM<School_View, SchoolSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("School", GridActionStandardTypesEnum.Create, Localizer["Create"],"", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.Edit, Localizer["Edit"], "", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.Delete, Localizer["Delete"], "", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.Details, Localizer["Details"], "", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.BatchEdit, Localizer["BatchEdit"], "", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.BatchDelete, Localizer["BatchDelete"], "", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.Import, Localizer["Import"], "", dialogWidth: 800),
                this.MakeStandardAction("School", GridActionStandardTypesEnum.ExportExcel, Localizer["Export"], ""),
            };
        }


        protected override IEnumerable<IGridColumn<School_View>> InitGridHeader()
        {
            return new List<GridColumn<School_View>>{
                this.MakeGridHeader(x => "test", width:100).SetFormat((a,b)=>{
                         var id = a.ID;
                    return this.UIService.MakeScriptButton(ButtonTypesEnum.Button,"出售账号",$"CheckSold('{id}')");
                }).SetHeader("出售账号"),
                this.MakeGridHeader(x => x.SchoolCode),
                this.MakeGridHeader(x => x.SchoolName),
                this.MakeGridHeader(x => x.SchoolType),
                this.MakeGridHeader(x => x.Remark),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<School_View> GetSearchQuery()
        {
            var query = DC.Set<School>()
                .CheckContain(Searcher.SchoolCode, x=>x.SchoolCode)
                .CheckContain(Searcher.SchoolName, x=>x.SchoolName)
                .CheckEqual(Searcher.SchoolType, x=>x.SchoolType)
                .Select(x => new School_View
                {
				    ID = x.ID,
                    SchoolCode = x.SchoolCode,
                    SchoolName = x.SchoolName,
                    SchoolType = x.SchoolType,
                    Remark = x.Remark,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class School_View : School{

    }
}
