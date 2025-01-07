[Root]
@RootInit:
RegistThrones;
RegistThronesState: Thrones_WallThrow;
return;

[Thrones_WallThrow]
@Main:
ThroneMove: 1, Boss_WallThrow;
WaitFrame: 140;
Exit;

# [Throne_Dstab]
# @Main:
# ThroneState: 1, Boss_Dstab;
# WaitFrame: 50;
# ThroneState: 2, Boss_Dstab;
# WaitFrame: 50;
# ThroneState: 3, Boss_Dstab;
# WaitFrame: 50;
# Exit;
