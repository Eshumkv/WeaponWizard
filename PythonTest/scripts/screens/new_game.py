#!/usr/bin/env python

"""
The new game screen
"""

from Microsoft.Xna.Framework import Color, Point
from Microsoft.Xna.Framework.Graphics import Texture2D

from WeaponWizard.Elements import Entity
from WeaponWizard.Elements.Enums import Message, Side
from WeaponWizard.Elements.Components import TransformComponent, RenderableComponent, SlideInAnimationComponent

from base import BaseScreen
import main as main_screen

class NewGameScreen (BaseScreen):
    def __init__(self):
        self.bgcolor = Color.Green

    def OnEnter(self, engine):
        BaseScreen.OnEnter(self, engine)

        enter_game_id = self.engine.Register(Message.Enter_Game, self.OnEnterGame)

        self.player = Entity() 
        renderable = self.player.AddComponent(RenderableComponent(self.engine.Content.Load[Texture2D]('default')))
        self.player.AddComponent(TransformComponent(200, 20, renderable.Texture.Width, renderable.Texture.Height))
        self.player.AddComponent(SlideInAnimationComponent(0.5, Side.Left))

        self.engine.AddEntity(self.player, 'player')


        self.player2 = Entity() 
        renderable2 = self.player2.AddComponent(RenderableComponent(self.engine.Content.Load[Texture2D]('default')))
        self.player2.AddComponent(TransformComponent(200, 70, renderable2.Texture.Width, renderable2.Texture.Height))
        self.player2.AddComponent(SlideInAnimationComponent(0.5, Side.Right))

        self.engine.AddEntity(self.player2, 'player2')

        self.engine.SendMsg(Message.Enter_Game, 'This is data')
        self.engine.Unregister(Message.Enter_Game, enter_game_id)
        self.engine.SendMsg(Message.Enter_Game, 'This will not be seen')

    def OnEnterGame(self, data):
        print(data)