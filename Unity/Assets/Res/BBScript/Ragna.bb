[Root]
@RootInit:
SetPos: 1100, 10000;
Gravity: 100000;
NumericType: MaxGravity, 150000;
NumericType: MaxFall, 45000;
NumericType: MaxJump, 2;
NumericType: MaxDash, 2;
NumericType: DashCount, 2;
NumericType: JumpCount, 2;
# 注册落地回调
RegistAirCheck;
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
# Move
RegistMove: (Rg_Idle)
  MoveType: None;
  EndMove:
RegistMove: (Rg_Run)
  MoveType: Move;
  EndMove:
RegistMove: (Rg_SquitToIdle)
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
RegistMove: (Rg_5B)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_5D)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_5C)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_TC_End)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_AirDash)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_GroundDash)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_24A)
  MoveType: Special;
   EndMove:
RegistMove: (Rg_ShouRyuuKenn)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_Throw)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_Hurt1)
  MoveType: HitStun;
  EndMove:
RegistMove: (Rg_HardDownRecovery)
  MoveType: HitStun;
  EndMove:
RegistMove: (Rg_IdleAnim)
  MoveType: Etc;
  EndMove:
return;


[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
UpdateFlip;
IdleAnim: Rg_IdleAnim;
InputBuffer: true;
DefaultWindow;
SetTransition: 'PreSquit';
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
return;


[Rg_Run]
@Trigger:
InAir: false;
InputType: RunHold;
return;

@Main:
#PreRun
UpdateFlip;
SetTransition: 'PreSquit';
InputBuffer: true;
DefaultWindow;
MoveX:13000;
BBSprite: 'PreRun_1', 1;
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
SetVelocityX: 0;
#Transition
TransitionWindow;
BBSprite: 'RunToIdle_1', 3;
BBSprite: 'RunToIdle_2', 3;
BBSprite: 'RunToIdle_3', 3;
BBSprite: 'RunToIdle_4', 3;
Exit;

[Rg_SquitToIdle]
@Trigger:
Transition: 'SquitToIdle';
return;

@Main:
DefaultWindow;
BBSprite: 'SquitToIdle_2', 2;
BBSprite: 'SquitToIdle_1', 2;
Exit;

[Rg_Squit]
@Trigger:
InAir: false;
InputType: SquatHold;
return;

@Main:
SetVelocityX: 0;
UpdateFlip;
InputBuffer: true;
DefaultWindow;
BeginIf: (TransitionCached: 'PreSquit')
  BBSprite: 'PreSquit_1', 3;
  BBSprite: 'PreSquit_2', 3;
  EndIf:
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
TransitionWindow;
BBSprite: 'PreSquit_2', 2;
BBSprite: 'PreSquit_1', 2;
Exit;

[Rg_AirBrone]
@Trigger:
InAir: true;
return;

@Main:
InputBuffer: true;
DefaultWindow;
AirMoveX: 15000;
UpdateFlip;
Gravity: 100000;
BeginIf: (TransitionCached: 'JumpToFall')
  BBSprite: 'JumpToFall_1', 4;
  BBSprite: 'JumpToFall_2', 4;
  BBSprite: 'JumpToFall_3', 4;
  BBSprite: 'JumpToFall_4', 4;
  BBSprite: 'JumpToFall_5', 4;
  EndIf:
BeginLoop: (InAir: true)
  BBSprite: 'Fall_1', 3;
  BBSprite: 'Fall_2', 3;
  EndLoop:
#MiddleLand
RemoveAirMoveX;
SetVelocityX: 0;
TransitionWindow;
BBSprite: 'Land_3', 3;
BBSprite: 'Land_4', 3;
BBSprite: 'Land_5', 3;
Exit;


[Rg_Jump]
@Trigger:
NumericCheck: JumpCount > 0;
InputType: JumpPressed;
return;

@Main:
SetVelocityX: 0;
BeginIf: (InAir: false)
  BBSprite: 'PreJump_1', 2;
  BBSprite: 'PreJump_2', 2;
  EndIf: 
AirMoveX: 15000;
UpdateFlip;
Gravity: 0;
NumericAdd: JumpCount, -1;
SetVelocityY: 20000;
InputBuffer: true;
BBSprite: 'Jump_1', 3;
BBSprite: 'Jump_2', 1;
GCWindow;
BBSprite: 'Jump_2', 2;
BBSprite: 'Jump_1', 3;
Gravity: 100000;
BBSprite: 'Jump_2', 3;
BBSprite: 'Jump_1', 3;
SetTransition: 'JumpToFall';
Exit;


[Rg_5B]
@Trigger:
InputType: 5LPPressed;
InAir: false;
return;

@Main:
SetTransition: 'PreSquit';
MarkerEvent: (Whiff_Start)
  InputBuffer: true;
  WhiffWindow;
  WhiffOption: 'Rg_GroundDash';
  EndMarkerEvent:
MarkerEvent: (Hit_Start)
  # 注册受击回调
  HurtNotify: Once
    HitParam: StopFrame, 0;
    HitParam: ShakeLength, 200;
    HitParam: ShakeFrame, 15;
    HitParam: PushBack_V, -13000;
    HitParam: PushBack_F, 38000;
    Hit_UpdateFlip;
    HitStun: 'Hurt2';
    EndNotify:
  # 注册攻击回调
  WaitHit:
    HitStop: 20, 10;
    ScreenShake: 30, 70;
    EndHit:
  EndMarkerEvent:
MarkerEvent: (Whiff_End)
  GCWindow;
  GCOption: 'Rg_5C';
  EndMarkerEvent:
StartTimeline;
Exit;

[Rg_5C]
@Trigger:
InputType: 5LPPressed;
InAir: false;
GCOption: 'Rg_5C';
return;

@Main:
SetTransition: 'PreSquit';
MarkerEvent: (Whiff_Start)
  InputBuffer: true;
  WhiffWindow;
  WhiffOption: 'Rg_GroundDash';
  EndMarkerEvent:
MarkerEvent: (Hit_Start)
  HurtNotify: Once
    HitParam: ShakeLength, 300;
    HitParam: ShakeFrame, 10;
    HitParam: StopFrame, 10;
    HitParam: VelocityY, 18000;
    HitParam: VelocityX, -8000;
    Hit_UpdateFlip;
    HitStun: 'Hurt1';
    EndNotify:
  WaitHit: 
    HitStop: 10, 10;
    ScreenShake: 170, 100;
    EndHit:
  EndMarkerEvent:
MarkerEvent: (Whiff_End)
  GCWindow;
  GCOption: 'Rg_5D';
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
SetTransition: 'PreSquit';
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
SetTransition: 'PreSquit';
MarkerEvent: (Whiff_Start)
  WhiffWindow;
  WhiffOption: 'Rg_GroundDash';
  EndMarkerEvent:
MarkerEvent: (Whiff_End)
  GCWindow;
  EndMarkerEvent:
StartTimeline;
Exit;


[Rg_AirDash]
@Trigger:
InAir: true;
return;

@Main:
InputBuffer: true;
DefaultWindow;
AirMoveX: 15000;
UpdateFlip;
Gravity: 100000;
BeginIf: (TransitionCached: 'JumpToFall')
  BBSprite: 'JumpToFall_1', 4;
  BBSprite: 'JumpToFall_2', 4;
  BBSprite: 'JumpToFall_3', 4;
  BBSprite: 'JumpToFall_4', 4;
  BBSprite: 'JumpToFall_5', 4;
  EndIf:
BeginLoop: (InAir: true)
  BBSprite: 'Fall_1', 3;
  BBSprite: 'Fall_2', 3;
  EndLoop:
#MiddleLand
RemoveAirMoveX;
SetVelocityX: 0;
TransitionWindow;
BBSprite: 'Land_3', 3;
BBSprite: 'Land_4', 3;
BBSprite: 'Land_5', 3;
Exit;


[Rg_GroundDash]
@Trigger:
NumericCheck: DashCount > 0;
InputType: DashPressed;
InAir: false;
return;

@Main:
NumericAdd: DashCount, -1;
# 冲刺充能
BeginIf: (NumericCheck: DashCount = 0)
  DashRecharge: 50;
  EndIf:
SetTransition: 'SquitToIdle';
MarkerEvent: (GC_Start)
  InputBuffer: true;
  GCWindow;
  GCOption: 'Rg_Jump';
  EndMarkerEvent:
StartTimeline;
Exit;


[Rg_Throw]
@Trigger:
InputType: 5MPPressed;
InAir: false;
return;

@Main:
SetVelocityX: 0;
SetVelocityY: 0;
BBSprite: 'Start_1', 3;
BBSprite: 'Start_2', 3;
# 投技判定
WaitThrow:
  ThrowPoint: -1500, 0; 
  HitStun: 'RgThrowHurt';
  EndThrow:
BBSprite: 'Start_3', 7;
#Throw Failed
BeginIf: (ThrowHurt: false)
  BBSprite: 'Failed_1', 15;
  BBSprite: 'Failed_2', 4;
  BBSprite: 'Failed_3', 4;
  BBSprite: 'Failed_4', 4;
  Exit;
  EndIf:
#Throw Successed
BBSprite: 'Success_1', 4;
BBSprite: 'Success_2', 4;
BBSprite: 'Success_3', 4;
BBSprite: 'Success_4', 4;
BBSprite: 'Success_5', 4;
ScreenShake: 170, 100;
BBSprite: 'Success_6', 4;
BBSprite: 'Success_7', 4;
BBSprite: 'Success_8', 10;
BBSprite: 'Success_9', 3;
BBSprite: 'Success_10', 3;
BBSprite: 'Success_11', 5;
BBSprite: 'Success_12', 3;
BBSprite: 'Success_13', 3;
# 将敌人抛飞
HitStun: 'Hurt3';
BBSprite: 'Success_13', 5;
BBSprite: 'Success_14', 4;
BBSprite: 'Success_15', 4;
BBSprite: 'Success_16', 4;
BBSprite: 'Success_17', 4;
BBSprite: 'Success_18', 4;
Reverse;
Exit;


[Rg_IdleAnim]
@Main:
InputBuffer: true;
TransitionWindow;
StartTimeline;
Exit;