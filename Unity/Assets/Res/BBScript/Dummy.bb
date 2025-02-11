[Root]
@RootInit:
DummyInit;
SetPos: 0, -8000;
Gravity: 100000;
# Cinemachine
CM_TargetGroup_Member: TG_Camera, 110, 250;
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
RegistMove: (Dummy_Hurt4)
  MoveType: HitStun;
  MoveFlag: Hurt4;
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
Shake: {Self.Shake_LengthX}, {Self.Shake_LengthY}, {Self.Shake_Frequency}, {Self.Shake_Frame};
BBSprite: 'hurt_1', 2;
BBSprite: 'hurt_3', 2;
BBSprite: 'hurt_5', {Self.HitStopFrame};
# PushBack: {Self.Push_V}, {Self.Push_F};
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
Gravity: 0;
# 帧冻结
HitStop: 0, {Self.HitStopFrame};
BBSprite: 'Frame_1', 1;
# 击飞效果
SetVelocityX: {Self.StartV_X};
SetVelocityY: {Self.StartV_Y};
BBSprite: 'Frame_2', 3;
BBSprite: 'Frame_3', 3;
Gravity: 60000;
BBSprite: 'Frame_2', 3;
BBSprite: 'Frame_3', 3;
BBSprite: 'Frame_2', 3;
BBSprite: 'Frame_3', 3;
Gravity: 120000;
# 上升
BeginLoop: (Velocity: Y > 150000)
  BBSprite: 'Frame_2', 3;
  BBSprite: 'Frame_3', 3;
  EndLoop:
# 开始下落
BBSprite: 'Frame_4', 3;
BBSprite: 'Frame_5', 3;
BBSprite: 'Frame_6', 3;
BBSprite: 'Frame_7', 3;
# 下落
BeginLoop: (InAir: true)
  BBSprite: 'Frame_8', 3;
  BBSprite: 'Frame_9', 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Frame_10', 2;
ScreenShake: 750, 750, 18000, 10; # 落地之后模拟弹地效果
BBSprite: 'Frame_10', 1;
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

[Dummy_Hurt4]
@Main:
Shake: {Self.Shake_LengthX}, 0, {Self.Shake_Frequency}, {Self.Shake_Frame};
HitStop: 0, {Self.HitStopFrame};
BBSprite: 'Hurt_1', 1;
PushBack: {Self.Push_V}, {Self.Push_F};
BBSprite: 'Hurt_2', 2;
BBSprite: 'Hurt_3', 2;
BBSprite: 'Hurt_4', 2;
BBSprite: 'Hurt_5', 3;
BBSprite: 'Hurt_6', 3;
BBSprite: 'Hurt_7', 30;
BBSprite: 'Recover_1', 3;
BBSprite: 'Recover_2', 3;
BBSprite: 'Recover_3', 3;
Exit;