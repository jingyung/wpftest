using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Wpftest
{
    public class KeyboardListener : IDisposable
    {
        private readonly Thread keyboardThread;

        //Here you can put those keys that you want to capture
        private readonly List<KeyState> numericKeys = new List<KeyState>
    {
 new KeyState(Key.LeftCtrl),
        new KeyState(Key.D0),
        new KeyState(Key.D1),
        new KeyState(Key.D2),
        new KeyState(Key.D3),
        new KeyState(Key.D4),
        new KeyState(Key.D5),
        new KeyState(Key.D6),
        new KeyState(Key.D7),
        new KeyState(Key.D8),
        new KeyState(Key.D9),
        new KeyState(Key.NumPad0),
        new KeyState(Key.NumPad1),
        new KeyState(Key.NumPad2),
        new KeyState(Key.NumPad3),
        new KeyState(Key.NumPad4),
        new KeyState(Key.NumPad5),
        new KeyState(Key.NumPad6),
        new KeyState(Key.NumPad7),
        new KeyState(Key.NumPad8),
        new KeyState(Key.NumPad9),
    new KeyState(Key.Space),
        new KeyState(Key.Enter)
    };

        private bool isRunning = true;

        public KeyboardListener()
        {
            keyboardThread = new Thread(StartKeyboardListener) { IsBackground = true };
            keyboardThread.Start();
        }

        private void StartKeyboardListener()
        {
            while (isRunning)
            {
                Thread.Sleep(1);
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (Application.Current.Windows.Count > 0)
                        {
                            foreach (var keyState in numericKeys)
                            {
                                if (Keyboard.IsKeyDown(keyState.Key) && !keyState.IsPressed) //
                                {
                                    keyState.IsPressed = true;
                                    KeyboardDownEvent?.Invoke(null, new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromDependencyObject(Application.Current.Windows[0]), 0, keyState.Key));
                                }

                                if (Keyboard.IsKeyUp(keyState.Key))
                                {
                                    keyState.IsPressed = false;
                                }
                            }
                        }
                    });
                }
            }
        }

        public event KeyEventHandler KeyboardDownEvent;

        private class KeyState
        {
            public KeyState(Key key)
            {
                this.Key = key;
            }

            public Key Key { get; }
            public bool IsPressed { get; set; }
        }

        public void Dispose()
        {
            isRunning = false;
            Task.Run(() =>
            {
                if (keyboardThread != null && !keyboardThread.Join(1000))
                {
                    keyboardThread.Abort();
                }
            });
        }
    }
}
