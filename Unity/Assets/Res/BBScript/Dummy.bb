[Root]
@RootInit:
DummyInit;
SetPos: 0, 1100;
# Gravity: 100000;
# Numeric
NumericType: Hertz, 60;
NumericType: MaxGravity, 150000;
NumericType: MaxFall, -450000;
# NumericChange
NumericChange: Hertz
  UpdateHertz;
  EndNumericChange:
# 创建碰撞盒: (Center), (Size)
AirCheckBox: 0, -1850, 1250, 1000;
# Move
RegistMove: (Dummy_Idle)
  MoveType: None;
  EndMove:
RegistMove: (Dummy_Hurt2)
  MoveType: HitStun;
  MoveFlag: Hurt2;
  EndMove:
SetFlip: Right;
GotoBehavior: 'Dummy_Idle';
return;

@BeforeReload:
# UpdateFlip: Once;
NumericSet: Hertz, 60;
return;

@LandCallback:
return;

[Dummy_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: -1000;
SetMarker: 'Loop';
BBSprite: 'Idle_1', 4;
BBSprite: 'Idle_2', 4;
BBSprite: 'Idle_3', 4;
BBSprite: 'Idle_4', 4;
BBSprite: 'Idle_5', 5;
BBSprite: 'Idle_6', 5;
BBSprite: 'Idle_7', 6;
BBSprite: 'Idle_8', 4;
BBSprite: 'Idle_9', 4;
BBSprite: 'Idle_10', 4;
BBSprite: 'Idle_11', 4;
BBSprite: 'Idle_12', 4;
GotoMarker: 'Loop';
Exit;

# 地面受击行为
[Dummy_Hurt2]
@Main:
ShakeX: {Self.ShakeX_Length}, {Self.ShakeX_Frequency}, {Self.ShakeX_Frame};
BBSprite: 'hurt_1', 2;
BBSprite: 'hurt_3', 2;
BBSprite: 'hurt_5', {Self.HitStopFrame};
PushBack: {Self.Push_V}, {Self.Push_F};
BBSprite: 'hurt_5', 6;
BBSprite: 'hurt_4', 3;
BBSprite: 'hurt_3', 3;
BBSprite: 'hurt_2', 3;
BBSprite: 'hurt_1', 3;
Exit;