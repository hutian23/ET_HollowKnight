[Root]
@RootInit:
DummyInit;
SetPos: 0, 2100;
Gravity: 100000;
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
RegistMove: (Dummy_Hurt3)
  MoveType: HitStun;
  MoveFlag: Hurt3;
  EndMove:
SetFlip: Right;
GotoBehavior: 'Dummy_Idle';
return;

@BeforeReload:
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

[Dummy_Hurt3]
@Main:
Shake: {Self.Shake_LengthX}, {Self.Shake_LengthY}, {Self.Shake_Frequency}, {Self.Shake_Frame};
SetVelocityX: 0;
SetVelocityY: 0;
Gravity: 60000;
BBSprite: 'Frame_1', {Self.HitStopFrame};
SetVelocityY: {Self.StartV_X};
SetVelocityX: {Self.StartV_Y};
BBSprite: 'Frame_2', 3;
BBSprite: 'Frame_3', 3;
Gravity: 120000;
BeginLoop: (Velocity: Y > 100000)
  BBSprite: 'Frame_2', 3;
  BBSprite: 'Frame_3', 3;
  EndLoop:
BBSprite: 'Frame_4', 3;
BBSprite: 'Frame_5', 3;
BBSprite: 'Frame_6', 3;
BBSprite: 'Frame_7', 3;
BeginLoop: (InAir: true)
  BBSprite: 'Frame_8', 3;
  BBSprite: 'Frame_9', 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Frame_10', 3;
ScreenShakeX: 950, 950, 18000, 10;
BBSprite: 'Frame_11', 3;
BBSprite: 'Frame_12', 3;
BBSprite: 'Frame_13', 3;
BBSprite: 'Frame_14', 3;
BBSprite: 'Frame_15', 3;
BBSprite: 'Frame_16', 3;
BBSprite: 'Frame_17', 25;
BBSprite: 'Frame_18', 3;
BBSprite: 'Frame_19', 3;
BBSprite: 'Frame_20', 3;
BBSprite: 'Frame_21', 3;
BBSprite: 'Frame_22', 3;
BBSprite: 'Frame_23', 3;
BBSprite: 'Frame_24', 3;
BBSprite: 'Frame_25', 3;
BBSprite: 'Frame_26', 3;
BBSprite: 'Frame_27', 3;
Exit;