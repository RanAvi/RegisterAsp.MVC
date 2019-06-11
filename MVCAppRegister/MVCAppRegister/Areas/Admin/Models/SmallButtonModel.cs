using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCAppRegister.Areas.Admin.Models
{
    public class SmallButtonModel
    {
      //@Html.ActionLink("Delete", "Delete", new { id=item.Id })

            //delete/edit/create
        public string Action { get; set; }

        //for browser optimize
        public string Text { get; set; }

        //the icon 
        public string Glyf { get; set; }

        //color for boostrap
        public string ButtonType { get; set; }

        // ? mean that is can be null 
        public int ? Id { get; set; }
        public int? ItemId { get; set; }
        public int? ProductId { get; set; }
        public int? SubscriptionId { get; set; }
        public string UserId { get; set; }

        public string ActionParameters {
            get
            {
                var param = new StringBuilder("?");
                if (Id != null && Id > 0)
                    param.Append(String.Format("{0}={1}&", "id", Id));
                if (ItemId != null && ItemId > 0)
                    param.Append(String.Format("{0}={1}&", "itemId", ItemId));
                if (ProductId != null && ProductId > 0)
                    param.Append(String.Format("{0}={1}&", "productId", ProductId));
                if (SubscriptionId != null && SubscriptionId > 0)
                    param.Append(String.Format("{0}={1}&", "subscriptionId", SubscriptionId));
                if (UserId != null && !UserId.Equals(0))
                    param.Append(String.Format("{0}={1}&", "UserId", UserId));

                return  param.ToString().Substring(0, param.Length - 1);
            }
                             
        }

    }
}