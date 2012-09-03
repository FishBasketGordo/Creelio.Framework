﻿namespace Creelio.Framework.Test.WebServices.Dto
{
    using System.Collections.Generic;
    using Creelio.Framework.UnitTesting;
    using Creelio.Framework.UnitTesting.Interfaces;
    using Creelio.Framework.WebServices.Dto;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Int32ResponseTests : IDataContractSerializationTests
    {
        [TestMethod]
        public void DataContractDeserializedObjectShouldEqualOriginal()
        {
            var source = new Int32Response
            {
                Item = 23,
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
                        return src.Item == other.Item
                            && src.AdditionalData.Count == other.AdditionalData.Count
                            && src.AdditionalData["Key"].Equals(other.AdditionalData["Key"]);
                    }
                    catch
                    {
                        return false;
                    }
                });
        }
    }
}