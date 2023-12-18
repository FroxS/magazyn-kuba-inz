using Warehouse.Core.Interface;
using Warehouse.Core.Models;
using Warehouse.Creator.Service;
using Warehouse.ViewModel.Pages;

namespace Warehouse.Creator.ViewModel
{
    public class WareHouseCreatorViewModel : WareHouseCreatorPageViewModel
    {
        #region Private properties

        #endregion

        #region Public properties

        public BaseStepViewModel ParentStepPage { get; }

        #endregion

        #region Constructors

        public WareHouseCreatorViewModel(IApp app, BaseStepViewModel parent) : base(app)
        {
            ParentStepPage = parent;
        }


        #endregion

        #region Helpers

        public override void OnPageOpen()
        {
            var hall = _hallService.GetAll().FirstOrDefault();
            if (hall == null)
            {
                Application.GetInnerDialogService().GetHallInnerDialog((o) => {

                    if (o == null)
                    {
                        ParentStepPage.SetPrevStep();
                        return;
                    }
                    var p1 = new WayPointObject(100, 100) { IsStartPoint = true };
                    var p2 = new WayPointObject(200, 100);
                    p1.AddConnection(ref p2);
                    o.WayPoints.Add(p1);
                    o.WayPoints.Add(p2);
                    Hall = o;
                });
            }
            else
            {
                Hall = _hallService.GetHallObject(hall.ID);
            }

            CanEdit = true; 
            ToSave = true;
        }

        #endregion
    }
}