using System;
using System.Collections.Generic;
using System.Text;
using AGT.Scenes;
using AGT.Forms;
using System.Threading;
using Microsoft.DirectX;
using AGT.Motion;

namespace VSG.PreviewSimulator
{
    public delegate void TickUpdateDelegate();
    public class PreviewSimulator: IDisposable
    {
        public TickUpdateDelegate TickUpdateCallback;

        private DDD_Playfield _scene;
        private AGT_CanvasControl _canvas_control;
        private SortedDictionary<int, List<AbstractPreviewEvent>> _scenario;
        private Dictionary<string, Vector3> _current_positions = new Dictionary<string, Vector3>();
        private List<string> _unit_list = new List<string>();
        public Dictionary<string, double> UnitMaxSpeeds = new Dictionary<string, double>();

        private Dictionary<string, Vector3> _reveal_positions = new Dictionary<string, Vector3>();

        private System.Threading.Timer _previewTimer;


        private float _mapScale = 815.4f;
        public float MapScale
        {
            get
            {
                lock (this)
                {
                    return _mapScale;
                }
            }
            set
            {
                lock (this)
                {
                    _mapScale = value;
                }
            }
        }
        
        
        private int _currentTick = 0;
        public int CurrentTick
        {
            get
            {
                lock (this)
                {
                    return _currentTick;
                }
            }
            set
            {
                lock (this)
                {
                    _currentTick = value;
                }
            }
        }

        private int _sim_speed = 1000;
        public int SimSpeed
        {
            get
            {
                lock (this)
                {
                    return _sim_speed;
                }
            }
            set
            {
                lock (this)
                {
                    _sim_speed = value;
                }
            }
        }

        public PreviewSimulator(AGT_CanvasControl control)
        {
            TickUpdateCallback = null;
            _scene = new AGT.Scenes.DDD_Playfield();
            _scene.UserControl = false;

            _canvas_control = control;
            _canvas_control.AddScene(_scene);

            _previewTimer = new System.Threading.Timer(new TimerCallback(this.OnTick), null, Timeout.Infinite, Timeout.Infinite);
            _scenario = new SortedDictionary<int, List<AbstractPreviewEvent>>();
        }

        
        public MoveEvent FindMoveEvent(int time, string unit_name)
        {
            if (_scenario.ContainsKey(time))
            {
                foreach (AbstractPreviewEvent evt in _scenario[time])
                {
                    if (evt is MoveEvent)
                    {
                        MoveEvent mv = (MoveEvent)evt;
                        if (mv.Name == unit_name)
                        {
                            return mv;
                        }
                    }
                }
            }
            return MoveEvent.Empty;
        }

        public bool SetSceneResources(string icon_library, string map_file)
        {

            if (!System.IO.File.Exists(map_file) )
            {
                System.Windows.Forms.MessageBox.Show("Please return to the Scenario Setup view and Check that your Map\nis pointing to valid resource files."
    , "Unable to Locate Scenario Resources", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                _scene.MapFile = string.Empty;
                _scene.ImageLibraryPath = string.Empty;
                return false;
            } else {
                _scene.MapFile = map_file;
            }

            if (!System.IO.File.Exists(icon_library))
            {
                System.Windows.Forms.MessageBox.Show("Please return to the Scenario Setup view and Check that your Icon Library\nis pointing to valid resource files."
                    , "Unable to Locate Scenario Resources", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                _scene.MapFile = string.Empty;
                _scene.ImageLibraryPath = string.Empty;
                return false;
            }
            else
            {
                _scene.ImageLibraryPath = icon_library;
            }
            return true;
        }
        public void SetSceneState(AGT.GameFramework.SceneState state)
        {
            _scene.State = state;
        }
        public void SetSceneScale(float scale)
        {
            _scene.PlayfieldScale = scale;
        }
        public void ClearScenario()
        {
            _scenario.Clear();
            UnitMaxSpeeds.Clear();
        }

        public void PauseSim()
        {
            _scene.Pause = true;
        }
        public void UnpauseSim()
        {
            _scene.Pause = false;
        }
        public void ClearScene()
        {
            _scene.RemovePawns();
        }
        public void StartSim(int tick)
        {
            CurrentTick = tick;
            _previewTimer.Change(0, this.SimSpeed);
        }

        public void StopSim()
        {
            _previewTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }


        public void OnTick(object state)
        {
            lock (this)
            {
                if (_scenario.ContainsKey(CurrentTick))
                {
                    foreach (AbstractPreviewEvent evt in _scenario[CurrentTick])
                    {
                        ExecuteEvent(evt);
                    }
                }
                TickUpdateCallback();
                CurrentTick++;
            }
        }

        public void StepTo(int step_finish, bool SimMotion)
        {
            for (int step_position = 0; step_position <= step_finish; step_position++)
            {
                if (_scenario.ContainsKey(step_position))
                {
                    foreach (AbstractPreviewEvent evt in _scenario[step_position])
                    {
                        StepThroughEvent(evt, (step_finish - step_position), SimMotion);
                    }
                }
            }
        }

        public void AddEvent(AbstractPreviewEvent scenario_event)
        {
            if (!_scenario.ContainsKey(scenario_event.Time))
            {
                _scenario.Add(scenario_event.Time, new List<AbstractPreviewEvent>());
            }
            _scenario[scenario_event.Time].Add(scenario_event);
        }


        public void AddEvent(AbstractPreviewEvent scenario_event, ReiterateEvent reiterate)
        {
            if (scenario_event is MoveEvent)
            {
                reiterate.AddEvent((MoveEvent)scenario_event);
            }
        }


        public void ExecuteEvent(AbstractPreviewEvent scenario_event)
        {
            if (scenario_event is RevealEvent)
            {
                RevealEvent evt = (RevealEvent)scenario_event;
                lock (_scene)
                {
                    if (!_reveal_positions.ContainsKey(evt.Name))
                    {
                        _reveal_positions.Add(evt.Name, new Vector3(evt.X, evt.Y, evt.Z));
                    }
                    _scene.AddMapObject(evt.Name, evt.Icon, evt.X, evt.Y, evt.Z, System.Drawing.Color.FromName(evt.ColorName));
                }
                return;
            }

            if (scenario_event is MoveEvent)
            {
                if (!_unit_list.Contains(scenario_event.Name))
                {
                    _unit_list.Add(scenario_event.Name);
                }
                MoveEvent evt = (MoveEvent)scenario_event;
                DoMove(evt, null);
                return;
            }

            if (scenario_event is ReiterateEvent)
            {
                ReiterateEvent evt = (ReiterateEvent)scenario_event;
                evt.ResetIndex();
                MoveEvent move = evt.CurrentEvent();

                DoMove(move, new System.Threading.ThreadStart(evt.StartNextEvent));
                _scene.ClearPawnSelection();

                return;
            }

        }

        public void StepThroughEvent(AbstractPreviewEvent scenario_event, int step_to, bool SimMotion)
        {

            if (scenario_event is RevealEvent)
            {
                RevealEvent evt = (RevealEvent)scenario_event;

                if (evt != RevealEvent.Empty)
                {
                    _scene.AddMapObject(evt.Name, evt.Icon, evt.X, evt.Y, evt.Z, System.Drawing.Color.FromName(evt.ColorName));
                    SetCurrentPosition(evt.Name, evt.X, evt.Y, evt.Z);
                    UpdateScene(evt);
                }
                return;
            }


            if (scenario_event is MoveEvent)
            {
                MoveEvent evt = (MoveEvent)scenario_event;
                AGT_LinearMotion motion_calculator = CreateMotionCalculator(evt);
                int move_duration = (int)GetMoveDuration(evt);
                if (motion_calculator != null)
                {
                    
                    if (step_to <= (evt.Time + move_duration))
                    {
                        // Interpolate position because we're in the middle of a move.
                        SetCurrentPosition(evt.Name, motion_calculator, step_to);                        
                    }
                    else
                    {
                        // We're not in the middle of this move, so just advance to the destination.
                        SetCurrentPosition(evt.Name, evt.X, evt.Y, evt.Z);
                    }
                    UpdateScene(evt, SimMotion);                    
             }
                return;
            }


            if (scenario_event is ReiterateEvent)
            {
                ReiterateEvent evt = (ReiterateEvent)scenario_event;
                evt.ResetIndex();
                Reiterate(evt, step_to, SimMotion);
            }
        }



        private void Reiterate(ReiterateEvent evt, int step_offset, bool SimMotion)
        {
            if (evt != ReiterateEvent.Empty)
            {
                MoveEvent current_move = evt.CurrentEvent();
                string unit_name = current_move.Name;
                int move_duration =  (int)(GetMoveDuration(current_move));
                if (step_offset <= move_duration)
                {
                    // We're in the middle of this move.
                    AGT_LinearMotion mc = CreateMotionCalculator(current_move);
                    SetCurrentPosition(unit_name,  mc,  step_offset);
                    UpdateScene(evt, SimMotion);

                }
                else if (step_offset > move_duration)
                {
                    // This move has already occurred, just advance to the next move
                    SetCurrentPosition(unit_name, current_move.X, current_move.Y, current_move.Z);
                    UpdateScene(evt, false);
                    evt.MoveToNextEvent();
                    Reiterate(evt, step_offset - move_duration, SimMotion);

                }
                return;
            }
                            
        }



        private float EventSpeed(MoveEvent evt)
        {
            if (UnitMaxSpeeds.ContainsKey(evt.Name))
            {
                return (float)((evt.Throttle * (float)UnitMaxSpeeds[evt.Name])/_mapScale);
            }
            return 0f;
        }

        #region IDisposable Members

        public void Dispose()
        {
            StopSim();
            _canvas_control.SuspendFramework();
            _scenario.Clear();
        }

        #endregion



        #region private 

        private double GetMoveDuration(MoveEvent evt)
        {
            return AGT_LinearMotion.CalculateMoveDuration(_current_positions[evt.Name].X, _current_positions[evt.Name].Y,
                    evt.X, evt.Y, EventSpeed(evt));
        }
        private void SetCurrentPosition(string unit_name, float x, float y, float z)
        {
            if (!_current_positions.ContainsKey(unit_name))
            {
                _current_positions.Add(unit_name, Vector3.Empty);
            }
            _current_positions[unit_name] = new Vector3(x, y, z);
        }

        private void SetCurrentPosition(string unit_name, Vector3 vector)
        {
            SetCurrentPosition(unit_name, vector.X, vector.Y, vector.Z);           
        }

        private void SetCurrentPosition(string unit_name, AGT_LinearMotion mc, int time)
        {
            SetCurrentPosition(unit_name, mc.InterpolatePosition(time));
        }

        private Vector3 GetCurrentPosition(string unit_name)
        {
            if (_current_positions.ContainsKey(unit_name))
            {
                return _current_positions[unit_name];
            }
            return Vector3.Empty;
        }
        public AGT_LinearMotion CreateMotionCalculator(MoveEvent evt)
        {
            if (_current_positions.ContainsKey(evt.Name) && evt != MoveEvent.Empty)
            {
                AGT_LinearMotion motion_calculator = new AGT_LinearMotion(_current_positions[evt.Name].X, _current_positions[evt.Name].Y, _current_positions[evt.Name].Z);

                motion_calculator.MoveTo(evt.X, evt.Y, evt.Z, EventSpeed(evt));

                return motion_calculator;
            }
            return null;

        }

        private void UpdateScene(RevealEvent evt)
        {
            lock (_scene)
            {
                string unit_name = evt.Name;
                Vector3 current = GetCurrentPosition(unit_name);

                _scene.SelectPawn(unit_name);
                _scene.MoveSelected(current.X, current.Y);
                _scene.ClearPawnSelection();
            }
        }

        public void DoMove(MoveEvent evt, System.Threading.ThreadStart callback)
        {
            if (_scene != null)
            {
                lock (_scene)
                {
                    _scene.SelectPawn(evt.Name);

                    if (callback != null)
                    {
                        _scene.MoveSelected(evt.X, evt.Y, (EventSpeed(evt) * (1000 / (float)SimSpeed)), callback);
                    }
                    else
                    {
                        _scene.MoveSelected(evt.X, evt.Y, (EventSpeed(evt) * (1000 / (float)SimSpeed)));
                    }
                    _scene.ClearPawnSelection();
                }
            }
        }

        private void UpdateScene(MoveEvent evt, bool simulate_motion)
        {
            lock (_scene)
            {
                string unit_name = evt.Name;
                Vector3 current = GetCurrentPosition(unit_name);

                _scene.SelectPawn(unit_name);
                if (simulate_motion)
                {
                    // Place object and put into motion.
                    _scene.MoveSelected(current.X, current.Y);
                    _scene.MoveSelected(evt.X, evt.Y, (EventSpeed(evt) * (1000 / (float)SimSpeed)));
                }
                else
                {
                    _scene.MoveSelected(current.X, current.Y);
                }
                _scene.ClearPawnSelection();
            }
   
        }
        private void UpdateScene(ReiterateEvent evt, bool simulate_motion)
        {
            lock (_scene)
            {
                MoveEvent move = evt.CurrentEvent();
                string unit_name = move.Name;

                Vector3 current = GetCurrentPosition(unit_name);
                if ((current != Vector3.Empty) && UnitMaxSpeeds.ContainsKey(unit_name))
                {
                    _scene.SelectPawn(unit_name);
                    if (simulate_motion)
                    {
                        // Move object and put into motion.
                        _scene.MoveSelected(current.X, current.Y);
                        if ((current.X != move.X) && (current.Y != move.Y))
                        {
                            _scene.MoveSelected(move.X, move.Y, (float)(EventSpeed(move) * (1000 / (float)SimSpeed)), new ThreadStart(evt.StartNextEvent));
                        }
                    }
                    else
                    {
                        _scene.MoveSelected(current.X, current.Y);
                    }
                    _scene.ClearPawnSelection();
                }
            }

        }
        

        #endregion


    }
}
