using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_SystemProgramming_
{
	public class Philosopher
	{
		private int _id;
		private Thread _philosopher;
		public Stopwatch _timerDuringEating;
        private int _timeForNextAction;
		public static Semaphore[] _semaphores = new Semaphore[5];
		private bool _isHungry;
		private Random _random;
		public Color philosopherState;
		private static bool[] _forks;
        private static object _checkLock = new object();
		public static int _lowerBorderInterval;
		public static int _upperBorderInterval;
		public static int totalTime;
		public static int pastTime = 0;


        public Philosopher(int id, int seed, bool[] forks)
		{
			_id = id;
            _timerDuringEating = new Stopwatch();
			_timeForNextAction = 0;
			_isHungry = false;
			_random = new Random(seed);
			philosopherState = Color.Red;
			_forks = forks;
			_philosopher = new Thread(LifeCycle);
			_philosopher.Start();
		}

		public void LifeCycle()
		{
			while (totalTime > pastTime)
			{
				while (_isHungry)
				{
					philosopherState = Color.Orange;
					if (CheckForks())
					{
						_forks[_id % _forks.Length] = true;
                        _forks[(_id + 1) % _forks.Length] = true;
                        philosopherState = Color.Green;
						//Philosopher is eating
						_timerDuringEating.Start();
						_timeForNextAction = _random.Next(_lowerBorderInterval, _upperBorderInterval);
                        
                        
						Thread.Sleep(_timeForNextAction);
                        _timerDuringEating.Stop();
                        ReleaseForks();

						_isHungry = false;
						_forks[_id] = false;
						_forks[(_id + 1) % _forks.Length] = false;
						
                    }
				}
				philosopherState = Color.Red;
				//Philosopher is thinking
				_timeForNextAction = _random.Next(_lowerBorderInterval, _upperBorderInterval);
				Thread.Sleep(_timeForNextAction);
				_isHungry = true;
			}
		}

		public bool CheckForks()
		{
			if (_semaphores[_id % _semaphores.Length].WaitOne())
			{
				if (_semaphores[(_id + 1) % _semaphores.Length].WaitOne())
				{
					return true;
				}
				else
				{
					_semaphores[_id % _semaphores.Length].Release();
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public void ReleaseForks()
		{
			_semaphores[_id % _semaphores.Length].Release();
			_semaphores[(_id + 1) % _semaphores.Length].Release();
		}

		public static void InitializeSemaphores()
		{
			for (int i = 0; i < _semaphores.Length; i++)
			{
                _semaphores[i] = new Semaphore(1, 1);
            }
		}
    }
}