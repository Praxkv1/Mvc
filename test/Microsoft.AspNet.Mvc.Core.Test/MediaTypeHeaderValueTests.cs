// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Microsoft.AspNet.Mvc.Core.Test
{
    public class MediaTypeHeaderValueTests
    {
        public static IEnumerable<object[]> SortValues
        {
            get
            {
                yield return new object[] {
                    new string[]
                        {
                            "application/*",
                            "text/plain",
                            "text/plain;q=1.0",
                            "text/plain",
                            "text/plain;q=0",
                            "*/*;q=0.8",
                            "*/*;q=1",
                            "text/*;q=1",
                            "text/plain;q=0.8",
                            "text/*;q=0.8",
                            "text/*;q=0.6",
                            "text/*;q=1.0",
                            "*/*;q=0.4",
                            "text/plain;q=0.6",
                            "text/xml",
                        },
                    new string[]
                        {
                            "text/plain",
                            "text/plain;q=1.0",
                            "text/plain",
                            "text/xml",
                            "application/*",
                            "text/*;q=1",
                            "text/*;q=1.0",
                            "*/*;q=1",
                            "text/plain;q=0.8",
                            "text/*;q=0.8",
                            "*/*;q=0.8",
                            "text/plain;q=0.6",
                            "text/*;q=0.6",
                            "*/*;q=0.4",
                            "text/plain;q=0",
                        }
                };
            }
        }

        [Theory]
        [MemberData(nameof(SortValues))]
        public void SortMediaTypeHeaderValuesByQFactor_SortsCorrectly(IEnumerable<string> unsorted, IEnumerable<string> expectedSorted)
        {
            // Arrange
            var unsortedValues =
                new List<MediaTypeHeaderValue>(
                    unsorted.Select(u => MediaTypeHeaderValue.Parse(u)));

            var expectedSortedValues =
                new List<MediaTypeHeaderValue>(
                    expectedSorted.Select(u => MediaTypeHeaderValue.Parse(u)));

            // Act
            var actualSorted = unsortedValues.OrderByDescending(m => m, MediaTypeHeaderValueComparer.QualityComparer).ToArray();

            // Assert
            for (int i = 0; i < expectedSortedValues.Count; i++)
            {
                Assert.True(MediaTypeHeaderValueComparer.QualityComparer.Compare(expectedSortedValues[i], actualSorted[i]) == 0);
            }
        }
    }
}
