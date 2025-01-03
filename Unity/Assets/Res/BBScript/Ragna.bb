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
# 创建碰撞盒: (Center), (Size)
AirCheckBox: 0, -2300, 1350, 1000;
# 落地回调
LandCallback:
  NumericSet: DashCount, 2;
  NumericSet: JumpCount, 2;
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
return;


[Rg_Idle]
@Trigger:
return;

@Main:
SetVelocityX: 0;
UpdateFlip;
#IdleAnim: Rg_IdleAnim;
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