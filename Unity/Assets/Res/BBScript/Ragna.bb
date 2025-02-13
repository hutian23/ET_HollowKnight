[Root]
@RootInit:
PlayerInit;
SetPos: 280000, -90000;
Gravity: 100000;
# Numeric
NumericType: Hertz, 60;
NumericType: MaxGravity, 150000;
NumericType: MaxFall, -450000;
NumericType: MaxJump, 5;
NumericType: MaxDash, 2;
NumericType: DashCount, 2;
NumericType: JumpCount, 2;
NumericType: Hertz, 60;
# NumericChange
NumericChange: Hertz
  UpdateHertz;
  EndNumericChange:
NumericChange: DashCount
  #OnGround---> DashRecharge
  BeginIf: (Numeric: DashCount <= 0), (InAir: false)
    DashRecharge: 2, 40;
    EndIf:
  EndNumericChange:
# 创建碰撞盒: (Center), (Size)
AirCheckBox: 0, -1850, 1250, 1000;
# Input
RegistInput: RunHold;
RegistInput: SquatHold;
RegistInput: 2LPPressed;
RegistInput: 5LPPressed;
RegistInput: 5MPPressed;
RegistInput: DashPressed;
RegistInput: 5LPHold;
RegistInput: ShouRyuKen;
RegistInput: JumpPressed;
RegistInput: QuickFallPressed;
RegistInput: JumpCancel;
# Move
RegistMove: (Rg_Idle)
  MoveType: None;
  EndMove:
RegistMove: (Rg_Land)
  MoveType: Move;
  EndMove:
RegistMove: (Rg_Run)
  MoveType: Move;
  EndMove:
RegistMove: (Rg_Squit)
  MoveType: Move;
  EndMove:
RegistMove: (Rg_AirBrone)
  MoveType: Move;
  EndMove:
RegistMove: (Rg_Jump)
  MoveType: Move;
  EndMove:
RegistMove: (Rg_JumpCancel)
  MoveType: Move;
  EndMove:
# RegistMove: (Rg_5B)
#   MoveType: Normal;
#   EndMove:
RegistMove: (Rg_6P)
  MoveType: Normal;
  EndMove:
# RegistMove: (Rg_5C)
#   MoveType: Normal;
#   EndMove:
RegistMove: (Rg_AirDash)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_GroundDash)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_24D)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_24D_Derive)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_26C)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_24A)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_Super)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_PlungingAttack)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_QuickFall)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_IdleAnim)
  MoveType: Etc;
  EndMove:
GotoBehavior: 'Rg_Idle';
return;

@BeforeReload:
UpdateFlip: Once;
NumericSet: Hertz, 60;
return;

@LandCallback:
RecordLandVelocity;
RemoveDashRecharge;
NumericSet: DashCount, 2;
NumericSet: JumpCount, 2;
SetVelocityY: -20000;
Gravity: 0;
return;

[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: -1000;
IdleAnim: Rg_IdleAnim, 300;
InputBuffer: true;
CancelWindow: Default;
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

[Rg_Land]
@Trigger:
Transition: 'AirToLand', true;
return;

@Main:
SetVelocityX: 0;
InputBuffer: true;
CancelWindow: Default;
BeginIf: (LandVelocity: 400000)
  # ScreenShakeX: 0, 120, 30000, 15;
  BBSprite: 'MiddleLand_1', 3;
  BBSprite: 'MiddleLand_2', 3;
  EndIf:
BBSprite: 'MiddleLand_3', 5;
BBSprite: 'MiddleLand_4', 4;
BBSprite: 'MiddleLand_5', 4;
Exit;

[Rg_Run]
@Trigger:
InAir: false;
InputType: RunHold;
return;

@Main:
#PreRun
UpdateFlip: Repeat;
InputBuffer: true;
CancelWindow: Default;
MoveX: 130000;
BBSprite: 'PreRun_1', 2;
BBSprite: 'PreRun_2', 2;
#Run
BeginLoop: (InputType: RunHold)
  BBSprite: 'Run_1', 4;
  BBSprite: 'Run_2', 4;
  BBSprite: 'Run_3', 4;
  BBSprite: 'Run_4', 4;
  BBSprite: 'Run_5', 4;
  BBSprite: 'Run_6', 4;
  EndLoop:
CancelMoveX;
SetVelocityX: 50000;
BBSprite: 'RunToIdle_1', 3;
BBSprite: 'RunToIdle_2', 3;
SetVelocityX: 0;
BBSprite: 'RunToIdle_3', 3;
CancelWindow: Transition;
BBSprite: 'RunToIdle_4', 3;
Exit;


[Rg_Squit]
@Trigger:
InAir: false;
InputType: SquatHold;
return;

@Main:
SetVelocityX: 0;
UpdateFlip: Repeat;
InputBuffer: true;
CancelWindow: Default;
BeginIf: (TransitionCached: 'NoPreSquat', false)
  BBSprite: 'PreSquit_1', 2;
  BBSprite: 'PreSquit_2', 2;
  EndIf:
SetTransition: 'SquatToJump';
BeginLoop: (InputType: SquatHold)
  BBSprite: 'Squit_1', 4;
  BBSprite: 'Squit_2', 4;
  BBSprite: 'Squit_3', 4;
  BBSprite: 'Squit_4', 4;
  BBSprite: 'Squit_5', 4;  
  BBSprite: 'Squit_6', 4;
  BBSprite: 'Squit_7', 4;
  BBSprite: 'Squit_6', 4;
  BBSprite: 'Squit_5', 4;
  BBSprite: 'Squit_4', 4;
  BBSprite: 'Squit_3', 4;
  BBSprite: 'Squit_2', 4;
  EndLoop:
RemoveTransition: 'SquatToJump';
CancelWindow: Transition;
BBSprite: 'PreSquit_2', 2;
BBSprite: 'PreSquit_1', 2;
Exit;

[Rg_AirBrone]
@Trigger:
InAir: true;
return;

@Main:
InputBuffer: true;
CancelWindow: Default;
UpdateFlip: Repeat;
Gravity: 100000;
AirMoveX: 150000;
# Airbrone
BeginLoop: (InAir: true)
  BBSprite: 'Fall_1', 3;
  BBSprite: 'Fall_2', 3;
  EndLoop:
# Land
SetTransition: 'AirToLand';
Exit;

[Rg_Jump]
@Trigger:
Numeric: JumpCount > 0;
InputType: JumpPressed;
return;

@Main:
SetVelocityX: 0;
# OnGround PreJump
BeginIf: (TransitionCached: 'SquatToJump', true)
  BBSprite: 'SquatToJump_1', 2;
  BBSprite: 'SquatToJump_2', 2;
  EndIf:
BeginIf: (InAir: false)
  BBSprite: 'PreJump_1', 2;
  BBSprite: 'PreJump_2', 2;
  EndIf:
# Jump
InputBuffer: true; 
UpdateFlip: Repeat;
Gravity: 0;
AirMoveX: 150000;
SetVelocityY: 200000;
NumericAdd: JumpCount, -1;
BBSprite: 'Jump_1', 3;
BBSprite: 'Jump_2', 3;
BBSprite: 'Jump_1', 3;
# Jump Cancel
CancelWindow: Gatling;
CancelOption: Rg_Jump;
Gravity: 100000;
BBSprite: 'Jump_2', 3;
BBSprite: 'Jump_1', 3;
# JumpToFall
BeginLoop: (InAir: true)
  BBSprite: 'JumpToFall_1', 3;
  BBSprite: 'JumpToFall_2', 3;
  BBSprite: 'JumpToFall_3', 3;
  BBSprite: 'JumpToFall_4', 3;
  BBSprite: 'JumpToFall_5', 3;
  Break;
  EndLoop:
#Land
SetTransition: 'AirToLand';
Exit;

[Rg_JumpCancel]
@Trigger: 
CancelOption: Rg_JumpCancel;
Numeric: JumpCount > 0;
InputType: JumpCancel;
return;

@Main:
SetVelocityX: 0;
# OnGround PreJump
BeginIf: (TransitionCached: 'SquatToJump', true)
  BBSprite: 'SquatToJump_1', 2;
  BBSprite: 'SquatToJump_2', 2;
  EndIf:
BeginIf: (InAir: false)
  BBSprite: 'PreJump_1', 2;
  BBSprite: 'PreJump_2', 2;
  EndIf:
# Jump
InputBuffer: true; 
Gravity: 0;
SetVelocityX: 50000;
SetVelocityY: 300000;
NumericAdd: JumpCount, -1;
BBSprite: 'Jump_1', 3;
BBSprite: 'Jump_2', 3;
BBSprite: 'Jump_1', 3;
# Jump Cancel
Gravity: 100000;
BBSprite: 'Jump_2', 3;
BBSprite: 'Jump_1', 3;
# JumpToFall
BeginLoop: (InAir: true)
  BBSprite: 'JumpToFall_1', 3;
  BBSprite: 'JumpToFall_2', 3;
  BBSprite: 'JumpToFall_3', 3;
  BBSprite: 'JumpToFall_4', 3;
  BBSprite: 'JumpToFall_5', 3;
  Break;
  EndLoop:
#Land
SetTransition: 'AirToLand';
Exit;

[Rg_5B]
@Trigger:
InputType: 5LPPressed;
InAir: false;
return;

@Main:
ApplyRootMotion: true;
MarkerEvent: (Whiff_Start)
  InputBuffer: true;
  CancelWindow: Whiff;
  CancelOption: Rg_GroundDash;
  EndMarkerEvent:
# MarkerEvent: (Hit_Start)
#   HurtNotify: Once
#     ShakeX: 1000, 40000, 10;
#     ScreenShakeX: 800, 20000, 10;
#     Hit_UpdateFlip;
#     HitStop: 15, 10;
#     HitParam: ShakeX_Length, 5000;
#     HitParam: ShakeX_Frequency, 50000;
#     HitParam: ShakeX_Frame, 12;
#     HitParam: HitStopFrame, 12;
#     HitStun: Hurt2;
#     EndNotify:
#   EndMarkerEvent:
MarkerEvent: (Whiff_End)
  CancelWindow: Gatling;
  CancelOption: Rg_5C;
  EndMarkerEvent:
StartTimeline;
Exit;

[Rg_5C]
@Trigger: 
InputType: 2LPPressed;
InAir: false;
# CancelOption: 'Rg_5C';
return;

@Main:
ApplyRootMotion: true;
MarkerEvent: (Whiff_Start)
  InputBuffer: true;
  CancelWindow: Whiff;
  CancelOption: Rg_GroundDash;
  EndMarkerEvent:
MarkerEvent: (Hit_Start)
  HurtNotify: Once
    ShakeX: 2000, 40000, 15;
    # ScreenShakeX: 1500, 35000, 15;
    HitStop: 5, 15;
    Hit_UpdateFlip;
    HitParam: ShakeX_Length, 8500;
    HitParam: ShakeX_Frequency, 60000;
    HitParam: ShakeX_Frame, 18;
    HitParam: HitStopFrame, 9;
    HitParam: Push_V, -200000;
    HitParam: Push_F, 950000;
    HitStun: Hurt3;
    EndNotify:
  EndMarkerEvent:
MarkerEvent: (Whiff_End)
  # GCWindow;
  # GCOption: 'Rg_5D';
  EndMarkerEvent:
StartTimeline;
Exit;

[Rg_5D]
@Trigger:
InputType: 5LPPressed;
InAir: false;
GCOption: 'Rg_5D';
return;

@Main:
MarkerEvent: (Whiff_Start)
  InputBuffer: true;
  WhiffWindow;
  WhiffOption: 'Rg_GroundDash';
  EndMarkerEvent:
MarkerEvent: (Whiff_End)
  GCWindow;
  GCOption: 'Rg_TC_End';
  EndMarkerEvent:
StartTimeline;
Exit;

[Rg_TC_End]
@Trigger:
InputType: 5LPPressed;
InAir: false;
GCOption: 'Rg_TC_End';
return;

@Main:
StartTimeline;
Exit;

[Rg_AirDashAttack]
@Trigger:
InAir: true;
InputType: 5LPPressed;
GCOption: 'Rg_AirDashAttack';
return;

@Main:
SetVelocityX: 10000;
SetVelocityY: 0;
Gravity: 0;
BBSprite: 'Attack_1', 3;
BBSprite: 'Attack_2', 3;
BBSprite: 'Attack_3', 3;
InputBuffer: true;
BBSprite: 'Attack_4', 3;
BBSprite: 'Attack_5', 3;
SetVelocityX: 70000;
BBSprite: 'Attack_6', 3;
BBSprite: 'Attack_7', 3;
SetVelocityX: 10000;
GCWindow;
GCOption: 'Rg_AirDashAttack';
BBSprite: 'Attack_8', 3;
BBSprite: 'Attack_9', 3;
BBSprite: 'Attack_10', 3;
BBSprite: 'Attack_11', 3;
BBSprite: 'Attack_12', 3;
Exit;


[Rg_6P]
@Trigger:
InAir: false;
InputType: 5LPPressed;
return;

@Main:
InputBuffer: true;
SetVelocityX: 0;
BBSprite: 'Start_1', 3;
BBSprite: 'Start_2', 3;
BBSprite: 'Start_3', 3;
BBSprite: 'Start_4', 3;
BBSprite: 'Start_5', 3;
# 这里开始，受击回调
HitNotify: Once # 对于同一对象，在持续帧内仅造成一次攻击(Repeat则为持续帧内，只要发生碰撞，每帧都会回调受击回调)
  CancelWindow: Gatling;
  CancelOption: Rg_JumpCancel;
  Shake: 500, 0, 8000, 18; # 振动
  HitStop: 0, 18; # 打击停顿
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 18;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 18;
  # HitStop结束后抛出的速度
  HitParam: StartV_X, -3000;
  HitParam: StartV_Y, 250000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt3;
  EndNotify:
# 攻击判定的持续帧
BBSprite: 'Active_1', 4;
DisposeWindow;
BBSprite: 'Recovery_1', 3;
BBSprite: 'Recovery_2', 3;
BBSprite: 'Recovery_3', 3;
BBSprite: 'Recovery_4', 3;
BBSprite: 'Recovery_5', 3;
BBSprite: 'Recovery_6', 3;
BBSprite: 'Recovery_7', 3;
Exit;

[Rg_AirDash]
@Trigger:
InAir: true;
InputType: DashPressed;
Numeric: DashCount > 0;
return;

@Main:
InputBuffer: true;
MarkerEvent: (GC_Start)
  CancelWindow: Gatling;
  CancelOption: Rg_Jump;
  # GCOption: 'Rg_AirDashAttack';
  # GCOption: 'Rg_PlungingAttack';
  EndMarkerEvent:
NumericAdd: DashCount, -1;
MarkerEvent: (RootMotion_Start)
  ApplyRootMotion: true;
  EndMarkerEvent:
MarkerEvent: (RootMotion_End)
  # Inertia
  CancelWindow: Transition;
  ApplyRootMotion: false;
  SetVelocityX: 80000;
  SetTransition: 'AirToLand';
  EndMarkerEvent:
StartTimeline;
Exit;

[Rg_GroundDash]
@Trigger:
InAir: false;
InputType: DashPressed;
Numeric: DashCount > 0;
return;

@Main:
InputBuffer: true;
SetVelocityY: 0;
SetVelocityX: 350000;
Gravity: 100000;
NumericAdd: DashCount, -1;
BBSprite: 'Dash_1', 3;
BBSprite: 'Dash_2', 3;
CancelWindow: Gatling;
CancelOption: Rg_Jump;
BBSprite: 'Dash_1', 3;
BBSprite: 'Dash_2', 3;
SetVelocityX: 200000;
BBSprite: 'Dash_1', 3;
SetVelocityX: 100000;
CancelOption: Rg_GroundDash;
BBSprite: 'DashEnd_1', 3;
SetVelocityX: 50000;
BBSprite: 'DashEnd_1', 6;
BBSprite: 'DashEnd_2', 3;
SetVelocityX: 0;
BBSprite: 'DashEnd_3', 1;
SetTransition: 'NoPreSquat';
CancelWindow: Transition;
BBSprite: 'DashEnd_3', 2;
BBSprite: 'DashEnd_4', 3;
BBSprite: 'DashEnd_5', 3;
BBSprite: 'DashEnd_6', 3;
Exit;

[Rg_PlungingAttack]
@Trigger:
InAir: true;
InputType: 2LPPressed;
return;

@Main:
Gravity: 0;
SetVelocityX: 100000;
SetVelocityY: 10000;
InputBuffer: true;
BBSprite: 'Pre_1', 3;
BBSprite: 'Pre_2', 3;
Gravity: 60000;
BBSprite: 'Pre_3', 3;
# WhiffWindow;
# WhiffOption: 'Rg_AirDash';
BBSprite: 'Pre_4', 3;
BBSprite: 'Attack_1', 2;
# DisposeWindow;
Gravity: 0;
SetVelocityX: 50000;
SetVelocityY: -700000;
HurtNotify: Once
  ShakeX: 500, 35000, 13;
  HitStop: 0, 13;
  Hit_UpdateFlip;
  HitParam: ShakeX_Length, 2000;
  HitParam: ShakeX_Frequency, 40000;
  HitParam: ShakeX_Frame, 13;
  HitParam: HitStopFrame, 8;
  HitParam: Push_V, -200000;
  HitParam: Push_F, 950000;
  HitStun: Hurt2;
  EndNotify:
BeginLoop: (InAir: true)
  BBSprite: 'Attack_2', 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Land_1', 5;
BBSprite: 'Land_2', 8;
BBSprite: 'Land_3', 5;
BBSprite: 'Land_4', 3;
BBSprite: 'Land_5', 3;
BBSprite: 'Land_6', 3;
BBSprite: 'Land_7', 3;
Exit;

[Rg_QuickFall]
@Trigger:
InAir: true;
InputType: QuickFallPressed;
return;

@Main:
Gravity: 0;
SetVelocityX: 0;
SetVelocityY: -3000000;
BeginLoop: (InAir: true)
  BBSprite: 'Fall_1', 3;
  EndLoop:
BBSprite: 'Land_1', 10;
BBSprite: 'Land_2', 4;
BBSprite: 'Land_3', 4;
#ToSquat
SetTransition: 'NoPreSquat';
InputBuffer: true;
CancelWindow: Transition;
BBSprite: 'Land_3', 2;
BBSprite: 'Land_4', 4;
BBSprite: 'Land_5', 4;
BBSprite: 'Land_6', 4;
Exit;

[Rg_24D]
@Trigger:
InputType: 5MPPressed; 
return;

@Main:
InputBuffer: true;
# Start
BeginIf: (InAir: false)
  SetVelocityX: 0;
  BBSprite: 'Start_1', 2;
  BBSprite: 'Start_2', 2;
  BBSprite: 'Start_3', 2;
  EndIf:
SetVelocityX: 180000;
SetVelocityY: 170000;
Gravity: 0;
BBSprite: 'Start_5', 4;
Gravity: 100000;
BBSprite: 'Start_5', 4;
# Active
CancelWindow: Gatling;
CancelOption: Rg_AirDash;
CancelOption: Rg_Jump;
HitNotify: Once
  HitStop: 0, 8; # 打击停顿
  Shake: 500, 0, 8000, 8; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 15;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 15;
  # HitStop结束后抛出的速度
  HitParam: Push_V, -200000;
  HitParam: Push_F, 800000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt4;
  EndNotify:
BBSprite: 'Active_1', 3;
BBSprite: 'Active_2', 3;
BBSprite: 'Active_3', 3;
BBSprite: 'Recover_1', 4;
# Recover
LogWarning: 'Enable';
BBSprite: 'Recover_2', 3;
CancelOption: Rg_26C;
CancelOption: Rg_24D_Derive;
BBSprite: 'Recover_3', 3;
BBSprite: 'Recover_4', 3;
BeginLoop: (InAir: true)
  BBSprite: 'Recover_5', 1;
  EndLoop:
DisposeWindow;
SetVelocityX: 0;
BBSprite: 'Recover_6', 4;
BBSprite: 'Recover_7', 4;
BBSprite: 'Recover_8', 4;
BBSprite: 'Recover_9', 4;
Exit;

[Rg_24D_Derive]
@Trigger:
CancelOption: Rg_24D_Derive;
InputType: 5MPPressed;
return;

@Main:
# Derive_Start
SetVelocityX: 80000;
SetVelocityY: 120000;
Gravity: 0;
BBSprite: 'Start2_1', 2;
# Derive_Active
HitNotify: Once
  HitStop: 5, 15; # 打击停顿
  Shake: 500, 0, 8000, 15; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1200;
  HitParam: Shake_LengthY, 1000;
  HitParam: Shake_Frequency, 10000;
  HitParam: Shake_Frame, 15;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 15;
  # HitStop结束后抛出的速度
  HitParam: StartV_X, -18000;
  HitParam: StartV_Y, 300000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt3;
  EndNotify:
BBSprite: 'Active2_1', 2;
Gravity: 100000;
BBSprite: 'Active2_1', 3;
BBSprite: 'Active2_2', 3;
# Derive_Recover
BBSprite: 'Recover2_1', 3;
BBSprite: 'Recover_4', 3;
BeginLoop: (InAir: true)
  BBSprite: 'Recover_5', 1;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Recover_6', 4;
BBSprite: 'Recover_7', 4;
BBSprite: 'Recover_8', 4;
BBSprite: 'Recover_9', 4;
Exit;

[Rg_24A]
@Trigger:
InputType: 5MPPressed;
InAir: false;
return;

@Main:
BBSprite: 'Start_3', 4;
SetVelocityX: 200000;
BBSprite: 'Start_4', 3;
BBSprite: 'Start_5', 3;
SetVelocityX: 100000;
HitNotify: Once
  HitStop: 0, 30; # 打击停顿
  Shake: 500, 0, 8000, 23; # 振动
  # 受击行为协程需要使用的变量
  HitParam: Shake_LengthX, 1100;
  HitParam: Shake_LengthY, 1100;
  HitParam: Shake_Frequency, 12000;
  HitParam: Shake_Frame, 23;
  # 受击者帧冻结(HitStop)的总帧长
  HitParam: HitStopFrame, 28;
  HitParam: StartV_X, -500000;
  HitParam: StartV_Y, 140000;
  # 受击时调整转向
  Hit_UpdateFlip;
  # 受击者进入哪个硬直状态
  HitStun: Hurt5;
  EndNotify:
BBSprite: 'Active_1', 5;
SetVelocityX: 50000;
BBSprite: 'Active_2', 3;
BBSprite: 'Active_2', 2;
SetVelocityX: 0;
BBSprite: 'Active_1', 5;
BBSprite: 'Recover_1', 4;
BBSprite: 'Recover_2', 4;
BBSprite: 'Recover_3', 3;
BBSprite: 'Recover_4', 3;
BBSprite: 'Recover_5', 3;
Exit;

[Rg_26C]
@Trigger:
CancelOption: Rg_26C;
InputType: 5LPPressed;
InAir: true;
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
Gravity: 0;
BBSprite: 'Start_1', 2;
BBSprite: 'Start_2', 2;
BBSprite: 'Start_3', 2;
BBSprite: 'Start_4', 2;
BBSprite: 'Start_5', 2;
BBSprite: 'Active_1', 4;
BBSprite: 'Active_2', 4;
BBSprite: 'Recover_1', 3;
BBSprite: 'Recover_2', 3;
Gravity: 100000;
BBSprite: 'Recover_3', 3;
BBSprite: 'Recover_4', 3;
Exit;

[Rg_Super]
@Trigger:
InputType: 5LPPressed;
return;

@Main:
SetVelocityX: 0;
BBSprite: 'Frame_1', 4;
RegistCounter: Cnt_1, 50;
BeginLoop: (Counter: Cnt_1 > 0)
  BBSprite: 'Frame_2', 4;
  BBSprite: 'Frame_3', 4;
  BBSprite: 'Frame_4', 4;
  EndLoop:
BBSprite: 'Frame_5', 3;
BBSprite: 'Frame_6', 3;
BBSprite: 'Frame_7', 3;
BBSprite: 'Frame_8', 3;
BBSprite: 'Frame_9', 3;
BBSprite: 'Frame_10', 3;
BBSprite: 'Frame_11', 3;
BBSprite: 'Frame_12', 3;
BBSprite: 'Frame_13', 3;
BBSprite: 'Frame_14', 3;
BBSprite: 'Frame_15', 4;
BBSprite: 'Frame_16', 4;
BBSprite: 'Frame_17', 4;
BBSprite: 'Frame_18', 4;
BBSprite: 'Frame_19', 4;
BBSprite: 'Frame_20', 4;
BBSprite: 'Frame_21', 4;
BBSprite: 'Frame_22', 3;
BBSprite: 'Frame_23', 3;
BBSprite: 'Frame_24', 5;
BBSprite: 'Frame_25', 3;
BBSprite: 'Frame_26', 3;
BBSprite: 'Frame_27', 4;
BBSprite: 'Frame_28', 3;
BBSprite: 'Frame_29', 3;
BBSprite: 'Frame_30', 4;
BBSprite: 'Frame_31', 4;
BBSprite: 'Frame_32', 4;
BBSprite: 'Frame_33', 4;
BBSprite: 'Frame_34', 5;
BBSprite: 'Frame_35', 4;
BBSprite: 'Frame_36', 4;
BBSprite: 'Frame_37', 3;
BBSprite: 'Frame_38', 3;
BBSprite: 'Frame_39', 3;
BBSprite: 'Frame_40', 3;
BBSprite: 'Frame_41', 3;
BBSprite: 'Frame_42', 3;
BBSprite: 'Frame_43', 4;
BBSprite: 'Frame_44', 4;
BBSprite: 'Frame_45', 4;
BBSprite: 'Frame_46', 4;
BBSprite: 'Frame_47', 5;
BBSprite: 'Frame_48', 5;
BBSprite: 'Frame_49', 5;
BBSprite: 'Frame_50', 15;
BBSprite: 'Frame_51', 4;
BBSprite: 'Frame_52', 4;
BBSprite: 'Frame_53', 4;
Exit;

[Rg_IdleAnim]
@Main:
InputBuffer: true;
CancelWindow: Transition;
StartTimeline;
Exit;