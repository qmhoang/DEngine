using System;
using System.Collections.Generic;
using DEngine.Core;
using DEngine.Extensions;
using libtcod;

namespace DEngine.States.Menus {
    public abstract class MenuScreen : GameState
    {        
        public class MenuEntry {
            public char CharShortcut { get; set; }
            public string Text { get; set; }
            public Point Position { get; set; }
            public Point FrameSize { get; set; }
            protected static readonly TCODColor normal = TCODColor.white;
            protected static readonly TCODColor selected = TCODColor.green;

            public delegate void MenuEntryEvent();
            public event MenuEntryEvent Selected;
            
            public virtual void OnSelected() {
                if (Selected != null)
                    Selected();
            }

            public MenuEntry() {}

            public MenuEntry(char charShortcut, string text, Point position) {
                CharShortcut = charShortcut;
                Text = text;
                Position = position;
                FrameSize = new Point(25, 3);
            }

            public MenuEntry(char charShortcut, string text, Point position, Point frameSize) {
                CharShortcut = charShortcut;
                Text = text;
                Position = position;
                FrameSize = frameSize;
            }

            public virtual void Draw(TCODConsole screen, bool isSelected) {
                screen.printFrame(Position.X, Position.Y, FrameSize.X, FrameSize.Y, true);
                if (isSelected)
                    screen.PrintColorString(String.Format("<{0}> - {1}", CharShortcut, Text), Position.X + 1, Position.Y + 1, selected);
                else
                    screen.PrintColorString(String.Format("[{0}] - {1}", CharShortcut, Text), Position.X + 1, Position.Y + 1, normal);                
                
            }

        }

        protected Dictionary<char, MenuEntry> menuEntries;
        protected char selectedEntry;

        protected MenuScreen() {
            menuEntries = new Dictionary<char, MenuEntry>();
            selectedEntry = 'a';
        }

        public override void HandleInput() {
            TCODKey key = TCODConsole.waitForKeypress(true);

            if (key.KeyCode == TCODKeyCode.Up) {
                selectedEntry--;

                if (selectedEntry - 'a' < 0)
                    selectedEntry = (char) ('a' + (menuEntries.Count - 1));

            } else if (key.KeyCode == TCODKeyCode.Down) {
                selectedEntry++;

                if (selectedEntry - 'a' >= menuEntries.Count)
                    selectedEntry = 'a';

            } else if (key.KeyCode == TCODKeyCode.Enter)
                menuEntries[selectedEntry].OnSelected();
            else {
                foreach (var entry in menuEntries) {
                    if (key.Character == entry.Key) {
                        entry.Value.OnSelected();
                    }
                }
            }
        } 
    }
}