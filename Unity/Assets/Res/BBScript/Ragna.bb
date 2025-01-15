[Root]
@RootInit:
PlayerInit;
SetPos: 2500, 10000;
Gravity: 100000;
# NumericChange
NumericChange: Hertz
  UpdateHertz;
  EndNumericChange:
# Numeric
NumericType: MaxGravity, 150000;
NumericType: MaxFall, 45000;
NumericType: MaxJump, 2;
NumericType: MaxDash, 2;
NumericType: DashCount, 2;
NumericType: JumpCount, 2;
NumericType: Hertz, 60;
# 创建碰撞盒: (Center), (Size)
AirCheckBox: 0, -1850, 1250, 1000;
# 落地回调
LandCallback:
  NumericSet: DashCount, 2;
  NumericSet: JumpCount, 2;
  Gravity: 0;
  SetVelocityY: -2000;
  EndCallback:
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
RegistMove: (Rg_5C)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_5D)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_TC_End)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_AirDashAttack)
  MoveType: Normal;
  EndMove:  
RegistMove: (Rg_AirDash)
  MoveType: Special;
  EndMove:
RegistMove: (Rg_GroundDash)
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
return;


[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
SetVelocityY: -1000;
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
Exit;

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
Numeric: JumpCount > 0;
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
  # HitStop: 15, 8;
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
MarkerEvent: (Whiff_Start)
  InputBuffer: true;
  WhiffWindow;
  WhiffOption: 'Rg_GroundDash';
  EndMarkerEvent:
MarkerEvent: (Hit_Start)
  # HitStop: 5, 12;
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

[Rg_AirDash]
@Trigger:
InAir: true;
InputType: DashPressed;
return;

@Main:
InputBuffer: true;
MarkerEvent: (GC_Start)
  GCWindow;
  GCOption: 'Rg_AirDashAttack';
  GCOption: 'Rg_PlungingAttack';
  EndMarkerEvent:
StartTimeline;
SetVelocityX: 10000;
Exit;

[Rg_GroundDash]
@Trigger:
InAir: false;
InputType: DashPressed;
return;

@Main:
StartTimeline;
Exit;

[Rg_PlungingAttack]
@Trigger:
InAir: true;
InputType: 2LPPressed;
return;

@Main:
Gravity: 0;
SetVelocityX: 8000;
SetVelocityY: 13000;
InputBuffer: true;
BBSprite: 'Pre_2', 3;
Gravity: 70000;
WhiffWindow;
WhiffOption: 'Rg_AirDash';
BBSprite: 'Pre_2', 5;
BBSprite: 'Pre_3', 5;
BBSprite: 'Pre_4', 4;
DisposeWindow;
Gravity: 0;
SetVelocityX: 4000;
SetVelocityY: -60000;
BBSprite: 'Attack_1', 2;
BeginLoop: (InAir: true)
  BBSprite: 'Attack_2', 3;
  EndLoop:
SetVelocityX: 0;
BBSprite: 'Land_1', 5;
BBSprite: 'Land_2', 8;
BBSprite: 'Land_3', 5;
BBSprite: 'Land_4', 4;
BBSprite: 'Land_5', 4;
BBSprite: 'Land_6', 4;
BBSprite: 'Land_7', 4;
Exit;

[Rg_QuickFall]
@Trigger:
InAir: true;
InputType: QuickFallPressed;
return;

@Main:
Gravity: 0;
SetVelocityX: 0;
SetVelocityY: -100000;
BeginLoop: (InAir: true)
  BBSprite: 'Fall_1', 3;
  EndLoop:
BBSprite: 'Land_1', 10;
BBSprite: 'Land_2', 4;
BBSprite: 'Land_3', 4;
BBSprite: 'Land_4', 4;
BBSprite: 'Land_5', 4;
BBSprite: 'Land_6', 4;
Exit;


[Rg_IdleAnim]
@Main:
InputBuffer: true;
TransitionWindow;
StartTimeline;
Exit;