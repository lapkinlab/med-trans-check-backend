using System;
using Model = Models.WayBills;
using Client = ClientModels.WayBills;

namespace ModelConverters.WayBills
{
    public static class WayBillSearchInfoConverter
    {
        public static Model.WayBillSearchInfo Convert(Client.WayBillSearchInfo clientSearchInfo)
        {
            if (clientSearchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientSearchInfo));
            }

            var modelSearchInfo = new Model.WayBillSearchInfo
            {
                Offset = clientSearchInfo.Offset,
                Limit = clientSearchInfo.Limit
            };

            return modelSearchInfo;
        }
    }
}