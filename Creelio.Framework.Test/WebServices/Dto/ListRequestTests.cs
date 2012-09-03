namespace Creelio.Framework.Test.WebServices.Dto
{
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.UnitTesting;
    using Creelio.Framework.UnitTesting.Interfaces;
    using Creelio.Framework.WebServices.Dto;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListRequestTests : IDataContractSerializationTests
    {
        [TestMethod]
        public void DataContractDeserializedObjectShouldEqualOriginal()
        {
            var source = new ListRequest<int>
            {
                Items = new List<int> { 1, 2, 3 },
                AdditionalData = new Dictionary<string, object> 
                { 
                    { "Key", "Value" }
                }
            };

            DtoAssert.DataContractDeserializedObjectIsEqualToOriginal(
                source,
                (src, other) =>
                {
                    try
                    {
                        var result = true;
                        result &= src.Items.Count() == other.Items.Count();

                        if (result)
                        {
                            for (int ii = 0; ii < src.Items.Count(); ii++)
                            {
                                result &= src.Items.ElementAt(ii) == other.Items.ElementAt(ii);
                            }
                        }

                        result &= src.AdditionalData.Count == other.AdditionalData.Count;
                        
                        if (result)
                        {
                            result &= src.AdditionalData["Key"].Equals(other.AdditionalData["Key"]);
                        }

                        return result;
                    }
                    catch
                    {
                        return false;
                    }
                });
        }
    }
}