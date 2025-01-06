[Root]
@RootInit:
RegistMove: (SlashRing_Idle)
  MoveType: None;
  EndMove:
return;

[SlashRing_Idle]
@Trigger:
return;

@Main:
BBRef: Timeline;
SetPos: [PosX], [PosY];
BBRef: Parser;
BBSprite: 'Slash_1', 3;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
BBSprite: 'Slash_2', 5;
BBSprite: 'Slash_3', 5;
Dispose;