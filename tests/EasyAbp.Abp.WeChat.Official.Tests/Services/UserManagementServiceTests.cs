﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.Abp.WeChat.Official.Services.User;
using EasyAbp.Abp.WeChat.Official.Services.User.Request;
using Shouldly;
using Xunit;

namespace EasyAbp.Abp.WeChat.Official.Tests.Services;

public class UserManagementServiceTests : AbpWeChatOfficialTestBase
{
    [Fact]
    public async Task Should_Return_All_User_OpenIds()
    {
        var userManagementService = await WeChatServiceFactory.CreateAsync<UserManagementWeService>();

        var openIdsResponse = await userManagementService.GetOfficialUserListAsync();
        openIdsResponse.ShouldNotBeNull();
        openIdsResponse.Count.ShouldBeGreaterThan(0);
        openIdsResponse.NextOpenId.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Should_Update_User_Remark_And_Return_Ok_Message()
    {
        var userManagementService = await WeChatServiceFactory.CreateAsync<UserManagementWeService>();

        var response = await userManagementService.UpdateUserRemarkAsync("on7qq1HZmDVgYTmzz8r3tayh-wqw", "RealZony");

        response.ErrorMessage.ShouldBe("ok");
        response.ErrorCode.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Return_User_Union_Info()
    {
        var userManagementService = await WeChatServiceFactory.CreateAsync<UserManagementWeService>();

        var response = await userManagementService.GetUserUnionInfoAsync("on7qq1HZmDVgYTmzz8r3tayh-wqw");

        response.ShouldNotBeNull();
        response.ErrorCode.ShouldBe(0);
        response.OpenId.ShouldBe("on7qq1HZmDVgYTmzz8r3tayh-wqw");
        response.Remark.ShouldBe("RealZony");
        response.SubscribeScene.ShouldBe(SubscribeSceneType.AddSceneQrCode);
    }

    [Fact]
    public async Task Should_Return_User_Union_Info_List()
    {
        var userManagementService = await WeChatServiceFactory.CreateAsync<UserManagementWeService>();

        var response = await userManagementService.BatchGetUserUnionInfoAsync(new List<GetUserUnionInfoRequest>
        {
            new GetUserUnionInfoRequest("on7qq1HZmDVgYTmzz8r3tayh-wqw"),
            new GetUserUnionInfoRequest("on7qq1H94tJeuwdC61iRsb6IQiAU")
        });

        response.ErrorMessage.ShouldBeNull();
        response.ErrorCode.ShouldBe(0);
        response.UserInfoList.Count.ShouldBe(2);
    }
}