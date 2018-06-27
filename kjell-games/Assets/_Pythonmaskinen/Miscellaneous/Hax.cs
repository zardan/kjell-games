﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM {

	[DisallowMultipleComponent]
	public class Hax : MonoBehaviour {
		
		void Update()
		{
			// Why use parantases when you can use multiple IF statements? B)
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				if (Input.GetKeyDown(KeyCode.RightArrow))
				{
					if (PMWrapper.currentLevel < PMWrapper.numOfLevels - 1)
					{
						if (PMWrapper.unlockedLevel < PMWrapper.currentLevel + 1)
							PMWrapper.unlockedLevel = PMWrapper.currentLevel + 1;
						PMWrapper.currentLevel += 1;
					}
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow))
				{
					if(PMWrapper.currentLevel > 0)
						PMWrapper.currentLevel -= 1;
				}
				else if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					PMWrapper.unlockedLevel = PMWrapper.numOfLevels - 1;
				}
			}
		}
	}
}