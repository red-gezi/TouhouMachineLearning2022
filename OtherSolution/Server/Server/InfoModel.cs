﻿using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Server
{
    public class UserState
    {
        public int Step { get; set; }
        public int Rank { get; set; }
    }
    public class HoldInfo
    {
        public HoldInfo(PlayerInfo playerInfo, PlayerInfo virtualOpponentInfo=null, IClientProxy client=null)
        {
            UserInfo = playerInfo;
            VirtualOpponentInfo = virtualOpponentInfo;
            Client = client;
            Rank = playerInfo.Rank;
            WinRate = playerInfo.WinRate;
            CollectionRate = 0;
            JoinTime = DateTime.Now;
        }
        public PlayerInfo UserInfo { get; set; }
        public PlayerInfo VirtualOpponentInfo { get; set; }
        public IClientProxy Client { get; set; }
        public int Rank { get; set; }
        public float WinRate { get; set; }
        public float CollectionRate { get; set; }
        public DateTime JoinTime { get; set; }
    }
    public class PlayerInfo
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string UID { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public float WinRate { get; set; }
        public Dictionary<string, int> Resource { get; set; }
        //决定游戏进程
        public UserState OnlineUserState { get; set; } = new UserState();
        public Dictionary<string, int> CardLibrary { get; set; }
        public int UseDeckNum { get; set; }
        public List<CardDeck> Decks { get; set; }
        [JsonIgnore]
        public CardDeck UseDeck => Decks[UseDeckNum];
        public PlayerInfo ShufflePlayerDeck()
        {
            Decks[UseDeckNum].CardIds = UseDeck.CardIds.OrderBy(i => new Random(DateTime.Now.GetHashCode()).Next()).ToList();
            return this;
        }
        public PlayerInfo() { }
        public PlayerInfo Creat(string account, string password, string title, List<CardDeck> deck, Dictionary<string, int> cardLibrary)
        {
            _id = Guid.NewGuid().ToString();
            UID = (MongoDbCommand.GetRegisterPlayerCount() + 1000).ToString();
            Account = account;
            Name = "村中人";
            Title = title;
            Decks = deck;
            Password = password.GetSaltHash(UID);
            CardLibrary = cardLibrary;
            Level = 0;
            Rank = 0;
            UseDeckNum = 0;
            Resource = new Dictionary<string, int>();
            OnlineUserState = new UserState();
            Resource.Add("faith", 0);
            Resource.Add("recharge", 0);
            return this;
        }
    }
    //卡牌不同版本的配置文件类型
    public class CardConfig
    {
        [BsonId]
        public string _id { get; set; }
        public DateTime UpdataTime { get; set; }
        public string Version { get; set; }
        public byte[] AssemblyFileData { get; set; }
        public byte[] SingleCardFileData { get; set; }
        public byte[] MultiCardFileData { get; set; }
        public CardConfig() { }
    }
    public class CardDeck
    {
        public string DeckName { get; set; }
        public int LeaderId { get; set; }
        public List<int> CardIds { get; set; }
        public CardDeck() { }
        public CardDeck(string DeckName, int LeaderId, List<int> CardIds)
        {
            this.DeckName = DeckName;
            this.LeaderId = LeaderId;
            this.CardIds = CardIds;
        }
    }
    //简易的数字卡牌量化模型
    public class SampleCardModel
    {
        public int CardID { get; set; } = 0;
        public int BasePoint { get; set; } = 0;
        public int ChangePoint { get; set; } = 0;
        public List<int> State { get; set; } = new List<int>();
        public SampleCardModel() { }
    }
    //对战记录模型
    public class AgainstSummary
    {
        [BsonId]
        public string _id { get; set; }
        public string AssemblyVerision { get; set; } = "";
        public PlayerInfo Player1Info { get; set; } 
        public PlayerInfo Player2Info { get; set; } 
        public int Winner { get; set; } = 0;
        public DateTime UpdateTime { get; set; }
        public List<TurnOperation> TurnOperations { get; set; } = new List<TurnOperation>();
        public class TurnOperation
        {
            public int RoundRank { get; set; }//当前小局数
            public int TurnRank { get; set; }//当前回合数
            public int TotalTurnRank { get; set; }//当前总回合数
            public bool IsOnTheOffensive { get; set; }//是否先手
            public bool IsPlayer1Turn { get; set; }//是否处于玩家1的操作回合

            public int RelativeStartPoint { get; set; }//玩家操作前双方的点数差
            public int RelativeEndPoint { get; set; }//玩家操作后双方的点数差  
            //0表示不投降，1表示玩家1投降，2表示玩家2投降
            public int SurrenderState { get; set; } = 0;
            public List<List<SampleCardModel>> AllCardList { get; set; } = new List<List<SampleCardModel>>();
            public PlayerOperation TurnPlayerOperation { get; set; }
            public List<SelectOperation> TurnSelectOperations { get; set; } = new List<SelectOperation>();
            public TurnOperation() { }
            public class PlayerOperation
            {
                public List<int> Operation { get; set; }
                public List<SampleCardModel> TargetcardList { get; set; }
                public int SelectCardID { get; set; }
                public int SelectCardIndex { get; set; }     //打出的目标卡牌索引

                public PlayerOperation() { }
            }
            public class SelectOperation
            {
                //操作类型 选择场地属性/从战场选择多个单位/从卡牌面板中选择多张牌/从战场中选择一个位置/从战场选择多片对战区域
                public List<int> Operation { get; set; }
                public int TriggerCardID { get; set; }
                //选择面板卡牌
                public List<int> SelectBoardCardRanks { get; set; }
                //换牌时洗入的位置
                public int WashInsertRank { get; internal set; }
                public bool IsPlayer1Select { get; set; }
                //换牌完成,true为玩家1换牌操作，false为玩家2换牌操作
                public bool IsPlay1ExchangeOver { get; set; }

                //选择单位
                public List<SampleCardModel> TargetCardList { get; set; }
                public List<int> SelectCardRank { get; set; }
                public int SelectMaxNum { get; set; }
                //区域
                public int SelectRegionRank { get; set; }
                public int SelectLocation { get; set; }
                public SelectOperation() { }
            }
        }
        /// <summary>
        /// 增加一个回合记录
        /// </summary>
        /// <param name="turnOperation"></param>
        public void AddTurnOperation(TurnOperation turnOperation)
        {
            Console.WriteLine("新增回合记录");
            TurnOperations.Add(turnOperation);
        }
        /// <summary>
        /// 增加一个回合玩家操作记录
        /// </summary>
        /// <param name="turnOperation"></param>
        public void AddPlayerOperation(TurnOperation.PlayerOperation playerOperation)
        {
            Console.WriteLine("新增回合玩家操作记录");
            TurnOperations.Last().TurnPlayerOperation = playerOperation;
        }

        /// <summary>
        /// 增加一个回合玩家选择记录
        /// </summary>
        /// <param name="turnOperation"></param>
        public void AddSelectOperation(TurnOperation.SelectOperation selectOperation)
        {
            Console.WriteLine("新增回合选择操作记录");
            TurnOperations.Last().TurnSelectOperations.Add(selectOperation);
        }

        public void AddStartPoint(int relativePoint)
        {
            Console.WriteLine("新增开始点数"+ relativePoint);
            TurnOperations.Last().RelativeStartPoint = relativePoint;
        }

        public void AddEndPoint(int relativePoint)
        {
            Console.WriteLine("新增结束点数" + relativePoint);
            TurnOperations.Last().RelativeEndPoint = relativePoint;
        }

        public void AddSurrender(int surrendrState)
        {
            Console.WriteLine("新增投降事件" );
            TurnOperations.Last().SurrenderState = surrendrState;
        }

        public void UploadAgentSummary(int p1Score, int p2Score)
        {
            UpdateTime = DateTime.Now;

            MongoDbCommand.InsertAgainstSummary(this);
        }
    }
}