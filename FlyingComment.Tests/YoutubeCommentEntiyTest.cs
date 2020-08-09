// <copyright file="YoutubeCommentEntiyTest.cs">Copyright ©  2020</copyright>
using System;
using FlyingComment.Model;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingComment.Model.Tests
{
    /// <summary>このクラスには、YoutubeCommentEntiy に対するパラメーター化された単体テストが含まれています</summary>
    [PexClass(typeof(YoutubeCommentEntiy))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class YoutubeCommentEntiyTest
    {
    }
}
