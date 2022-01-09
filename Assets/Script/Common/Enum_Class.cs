using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Building_Type
{
    Small,
    Middle,
    Big,
    BathHouse,
    Church,
    Karaoke,
    School,
    Company,
    Nothing,
};
public enum Tile_Type
{
    None = 0,
    White = 1,
    Red = 2,
    Blue = 3,
}
public enum Citizen_Type
{
    Normal = 0,
    Young,
    Old,
    The,
    Tourist
}

public enum CitizenColor
{
    White,
    Red,
    Blue,
}

public enum CitizenAnimState
{
    Stop,
    Back,
    Side,
}

public enum NodeType
{
    // Building
    Home,
    BathHouse,
    Church,
    Karaoke,
    School,
    Company,
    Nothing,

    // Tile
    None,
    White,
    Red,
    Blue,

    Tile_Offset = None,
}

public enum Ballone
{
    Move,
    Idle,
}

public enum ItemState
{
    None,
    Cleaner,
    GiftCntAdd,
    Cityhall,
    BanArea,
    CleanMan,
    ForceGoHome,
    Delivery
}

public enum BGMEnum
{
    /// <summary> 로고 BGM </summary>
    Logo,
    /// <summary> 스테이지 선택 </summary>
    StageSelect,
    /// <summary> 인게임 BGM </summary>
    InGame,
    /// <summary> 튜토리얼 </summary>
    Tutorial
}
public enum SeEnum
{
    /// <summary> 터치음 </summary>
    Touch,
    /// <summary> 스테이지 입장 </summary>
    Enter_Stage,
    /// <summary> 주민이동 </summary>
    Citizen_move,
    /// <summary> 차량 이동 </summary>
    Car_move,
    /// <summary> 차량 등장 </summary>
    Car_horn,
    /// <summary> 택배 전달 </summary>
    Deliver,
    /// <summary> 빨간 물방울 떨어지기 </summary>
    Drop_red,
    /// <summary> 아이템 사용 </summary>
    UseItem,
    /// <summary> 금지구역 설치 </summary>
    UseBanner,
    /// <summary> 방역이 설치 </summary>
    UseCleaner,
    /// <summary> 물자구입 </summary>
    UseSupply,
    /// <summary> 강제이주 </summary>
    UseForced,
    /// <summary> 로켓배송 </summary>
    UseRocket,
    /// <summary> 세탁 </summary>
    Clean,
    /// <summary> 세탁 완료 </summary>
    Clean_finish,
    /// <summary> 건물 입장 </summary>
    Building_enter,
    /// <summary> 건물 퇴장 </summary>
    Building_exit,
    /// <summary> 교회 예배 </summary>
    Church_Pray,
    /// <summary> 노래방 노래 </summary>
    Sing_Sing,
    /// <summary> 회사 터치 </summary>
    Office_Touch,
    /// <summary> 세금 증가 </summary>
    GetTax,
    /// <summary> 학교 모집 </summary>
    School_collect,
    /// <summary> 학교 해산 </summary>
    School_spread,
    /// <summary> 주민 터치 </summary>
    Citizen_Touch,
    /// <summary> 결과_성공 </summary>
    Result_Success,
    /// <summary> 결과_실패 </summary>
    Result_Fail,
    /// <summary> 별획득1 </summary>
    Star_Rating_1,
    /// <summary> 별획득2 </summary>
    Star_Rating_2,
    /// <summary> 별획득3 </summary>
    Star_Rating_3,
}

 
   
   
   
    
    
    

public class Enum_Class : MonoBehaviour
{

}
