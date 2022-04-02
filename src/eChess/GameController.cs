﻿using eChessServer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Point = System.Drawing.Point;

namespace eChess
{
    class GameController
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri(Constants.ApiBaseAddress),
            Timeout = TimeSpan.FromMilliseconds(250000)
        };



        public static async Task<bool> PostMove(Guid gameID, Guid playerGuid, Point currentPos, Point newPos)
        {
            try
            {
                var response = await client.GetAsync("/Game/PostMove?gameID=" + gameID + "&playerGuid=" + playerGuid + "&currentPosX=" + currentPos.X + "&currentPosY=" + currentPos.Y + "&newPosX=" + newPos.X + "&newPosY=" + newPos.Y);
                var content = response.Content.ReadAsStringAsync().Result;
                if (content != "true")
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static async Task<MoveEntity> ReceiveMove(Guid gameID, Guid playerGuid)
        {
            try
            {
                await Task.Delay(300);
                var response = await client.GetAsync("/Game/ReceiveMove?gameID=" + gameID + "&playerGuid=" + playerGuid).Result.Content.ReadAsStringAsync();
                MoveEntity move = JsonConvert.DeserializeObject<MoveEntity>(response);
                if (move == new MoveEntity())
                {
                    return new MoveEntity();
                }
                else if (move.currentPos != Point.Empty || move.newPos != Point.Empty)
                {
                    return move;
                }
            }
            catch
            {

            }
            return new MoveEntity();
        }
    }
}