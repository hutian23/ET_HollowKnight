[Root]
@RootInit:
Thrones_Init;
Thrones_Goto: Thrones_Dstab;
return;

[Thrones_Dstab]
@Main:
Thrones_SubState: 1, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_SubState: 2, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_Goto: Thrones_Dash;
return;

[Thrones_Dash]
@Main:
# Throne_1
Thrones_Param: 1, Flip, Left;
Thrones_Param: 1, PosX, -10500;
Thrones_Param: 1, PosY, 3100;
Thrones_Param: 1, VelX, -100000;
Thrones_Param: 1, ReachX, 9500;
Thrones_Param: 1, Direction, Right;
Thrones_SubState: 1, Boss_Dash;
Thrones_WaitFrame: 60;
#Throne_2
Thrones_Param: 2, Flip, Right;
Thrones_Param: 2, PosX, 10500;
Thrones_Param: 2, PosY, 3100;
Thrones_Param: 2, VelX, -100000;
Thrones_Param: 2, ReachX, -9500;
Thrones_Param: 2, Direction, Left;
Thrones_SubState: 2, Boss_Dash;
Thrones_WaitFrame: 60;
#Throne_3
Thrones_Param: 3, Flip, Left;
Thrones_Param: 3, PosX, -10500;
Thrones_Param: 3, PosY, 3100;
Thrones_Param: 3, VelX, -100000;
Thrones_Param: 3, ReachX, 9500;
Thrones_Param: 3, Direction, Right;
Thrones_SubState: 3, Boss_Dash;
Thrones_WaitFrame: 60;
Thrones_Goto: Thrones_TwoDash;

[Thrones_TwoDash]
@Main:
# Throne_1
Thrones_Param: 1, Flip, Left;
Thrones_Param: 1, PosX, -10500;
Thrones_Param: 1, PosY, 3100;
Thrones_Param: 1, VelX, -100000;
Thrones_Param: 1, ReachX, 9500;
Thrones_Param: 1, Direction, Right;
Thrones_SubState: 1, Boss_Dash;
#Throne_2
Thrones_Param: 2, Flip, Right;
Thrones_Param: 2, PosX, 10500;
Thrones_Param: 2, PosY, 3100;
Thrones_Param: 2, VelX, -100000;
Thrones_Param: 2, ReachX, -9500;
Thrones_Param: 2, Direction, Left;
Thrones_SubState: 2, Boss_Dash;
Thrones_WaitFrame: 60;
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_Goto: Thrones_Dash_Throw_Dstab;

[Thrones_Dash_Throw_Dstab]
@Main:
# Throne_1
Thrones_Param: 1, Flip, Left;
Thrones_Param: 1, PosX, -10500;
Thrones_Param: 1, PosY, 3100;
Thrones_Param: 1, VelX, -100000;
Thrones_Param: 1, ReachX, 9500;
Thrones_Param: 1, Direction, Right;
Thrones_SubState: 1, Boss_Dash;
Thrones_WaitFrame: 60;
# Throne_2
Thrones_Param: 2, Flip, Left;
Thrones_Param: 2, PosX, 14600;
Thrones_Param: 2, PosY, 5000;
Thrones_SubState: 2, Boss_WallThrow;
Thrones_WaitFrame: 60;
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_Goto: Thrones_TwoDstab;

[Thrones_TwoDstab]
@Main:
Thrones_SubState: 1, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_SubState: 2, Boss_Dstab;
Thrones_WaitFrame: 55;
#Throne_3
Thrones_Param: 3, Flip, Left;
Thrones_Param: 3, PosX, -10500;
Thrones_Param: 3, PosY, 3100;
Thrones_Param: 3, VelX, -100000;
Thrones_Param: 3, ReachX, 9500;
Thrones_Param: 3, Direction, Right;
Thrones_SubState: 3, Boss_Dash;
Thrones_WaitFrame: 60;
Thrones_Goto: Thrones_WallThrow;

[Thrones_WallThrow]
@Main:
# Throne_1
Thrones_Param: 1, Flip, Left;
Thrones_Param: 1, PosX, 14600;
Thrones_Param: 1, PosY, 5000;
Thrones_SubState: 1, Boss_WallThrow;
# Throne_2
Thrones_Param: 2, Flip, Right;
Thrones_Param: 2, PosX, -14600;
Thrones_Param: 2, PosY, 5000;
Thrones_SubState: 2, Boss_WallThrow;
Thrones_WaitFrame: 55;
# Throne_3
Thrones_SubState: 3, Boss_Dstab;
Thrones_WaitFrame: 55;
Thrones_Goto: Thrones_Dstab;