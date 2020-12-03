﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ContentCenter.IServices;
using ContentCenter.Model.ThirdPart.Baidu;
using IQB.Util;
using IQB.Util.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ContentCenter.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BaiduController : CCBaseController
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;
        private IBaiduPanService _baiduPanService;
        public BaiduController(
           IWebHostEnvironment webHostEnvironment,
           IConfiguration configuration,
           IBaiduPanService baiduPanService
         )
        {
            _configuration = configuration;
          
            _webHostEnvironment = webHostEnvironment;
            _baiduPanService = baiduPanService;
        }

        [HttpPost]
        public ResultEntity<panAccessToken> getAvaliableAccessToken()
        {
            ResultEntity<panAccessToken> result = new ResultEntity<panAccessToken>();
            try
            {
                    _baiduPanService.getAvaliableAccessToken();
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        

        [HttpGet]
        public ResultEntity<panAccessToken> panCallback()
        {
            ResultEntity<panAccessToken> result = new ResultEntity<panAccessToken>();
            try
            {
                NLogUtil.cc_InfoTxt($"pancallback comming");
                string code = HttpContext.Request.Query["code"];
                if (!string.IsNullOrEmpty(code))
                {
                  //  NLogUtil.cc_InfoTxt($"pancallback:{code}");
                    string url = @$"https://openapi.baidu.com/oauth/2.0/token?grant_type=authorization_code&code={code}&client_id=YunvBnAcNTqOr3SbAGGoFNx9TAYwfX2k&client_secret=irhor1R0COYhjm8x2CyfUBGGb1Ggt07T&redirect_uri=http://ccapi.iqianba.cn/baidu/pancallback";
                    NLogUtil.cc_InfoTxt($"pancallback:{url}");

                    panAccessToken accessToken = HttpUtil.Get<panAccessToken>(url);
                    result.Entity = accessToken;

                 
                    _baiduPanService.SaveAccessToken(accessToken);

                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultEntity<panFilemetaList> run(panQueryFile query)
        {
            var fList = this.fileList(query);
            var list = fList.Entity.list;
            query.fsIds = new List<string>();
            foreach (var item in list)
            {
                if (item.isdir == 0)
                    query.fsIds.Add(item.fs_id);
            }

            return this.filemetaList(query);
        }

        [HttpPost]
        public ResultEntity<panFileList> fileList(panQueryFile query)
        {
            ResultEntity<panFileList> result = new ResultEntity<panFileList>();
            try
            {              
                string url = $"https://pan.baidu.com/rest/2.0/xpan/multimedia?method=listall&path={query.rootPath}&access_token={query.accessToken}";
                var list = HttpUtil.Get<panFileList>(url);
                result.Entity = list; 
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
        [HttpPost] 
        public ResultEntity<panFilemetaList> filemetaList(panQueryFile query)
        {
            ResultEntity<panFilemetaList> result = new ResultEntity<panFilemetaList>();
            try
            {
                string fsids = string.Join(",",query.fsIds);
                string url = @$"https://pan.baidu.com/rest/2.0/xpan/multimedia?method=filemetas&
path={query.rootPath}&fsids=[{fsids}]&dlink=1&access_token={query.accessToken}";
                var list = HttpUtil.Get<panFilemetaList>(url);
                result.Entity = list;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public ResultNormal downloadFileByUrl(panReqDownload reqDownload)
        {
            ResultNormal result = new ResultNormal();
            try
            {
                string filePath = _webHostEnvironment.ContentRootPath + _configuration["baiduConfig:downloadPath"] + reqDownload.filename;
                using (HttpClient httpClient = new HttpClient())
                {
                    var clientResult = httpClient.GetAsync(reqDownload.url).Result;
                    while (clientResult.StatusCode == HttpStatusCode.Redirect)
                    {
                        var url = clientResult.Headers.Location.AbsoluteUri;
                        clientResult = httpClient.GetAsync(url).Result;
                    }
                    var bytes = clientResult.Content.ReadAsByteArrayAsync().Result;
                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                    }
                }
             
            }
          
            catch (Exception ex)
            {
               
                result.ErrorMsg = ex.Message;
            }
            return result;
          
        }

    



    }
}