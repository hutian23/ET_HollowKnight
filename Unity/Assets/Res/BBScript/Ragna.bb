[Root]
@RootInit:
PlayerInit;
SetPos: 5500, 10000;
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
RegistMove: (Rg_5B)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_5C)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_AirDash)
  MoveType: Normal;
  EndMove:
RegistMove: (Rg_GroundDash)
  MoveType: Normal;
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
MarkerEvent: (Hit_Start)
  HurtNotify: Once
    ShakeX: 400, 30000, 10;
    ScreenShakeX: 100, 20000, 10;
    Hit_UpdateFlip;
    HitStop: 15, 10;
    HitParam: ShakeX_Length, 1200;
    HitParam: ShakeX_Frequency, 22000;
    HitParam: ShakeX_Frame, 15;
    HitParam: HitStopFrame, 10;
    HitParam: Push_V, -80000;
    HitParam: Push_F, 450000;
    HitStun: Hurt2;
    EndNotify:
  EndMarkerEvent:
MarkerEvent: (Whiff_End)
  CancelWindow: Gatling;
  CancelOption: Rg_5C;
  EndMarkerEvent:
StartTimeline;
Exit;

[Rg_5C]
@Trigger: 
InputType: 5LPPressed;
InAir: false;
CancelOption: 'Rg_5C';
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
    ShakeX: 800, 30000, 15;
    ScreenShakeX: 1000, 35000, 15;
    HitStop: 5, 8;
    Hit_UpdateFlip;
    HitParam: ShakeX_Length, 1500;
    HitParam: ShakeX_Frequency, 25000;
    HitParam: ShakeX_Frame, 15;
    HitParam: HitStopFrame, 6;
    HitParam: Push_V, -200000;
    HitParam: Push_F, 950000;
    HitStun: Hurt2;
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


[Rg_IdleAnim]
@Main:
InputBuffer: true;
CancelWindow: Transition;
StartTimeline;
Exit;