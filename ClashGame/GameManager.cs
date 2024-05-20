﻿using System.Collections.Generic;
using System.Windows.Controls;

namespace ClashGame
{
    sealed class GameManager
    {
        //синглтон сраный
        private static GameManager? _instance;
        public static GameManager Instance
        {
            get
            {
                _instance ??= new GameManager();
                return _instance;
            }
        }

        private GameManager()
        {
        }

    }
}
