README for ScenarioController
Adam Dziki
As Of: 7/28/2006

This is the rough draft of the Scenario Controller Component.  
It's goal is to Read in events from a scenario file (forthcoming, 
for now it receives hard coded data populated in the Scenario Reader
class).  This event info is stored into the EventQueue in the
QueueManager (the "Queue" is actually a dictionary of lists).
The program.cs file instantiates the QueueManager and ScenarioReader (and
the EventListener, once I talk with Gabe about MoveComplete events)
and then iterates a loop, in which ticks are incremented, and if
and event takes place at that tick, the events are removed
from the EventQueue and sent to the stub server.

To Run:
Open a command prompt, navigate to the directory:
~\src\TestStubs\NetworkServerStub\bin\Debug (where ~ is the DDD repository)

run the following:

 NetworkServerStub.exe 9999 SimulationModel.xml
 
 In VS .Net, for the Scenario controller, make sure it has the following command
 line parameters: adziki 9999 SimulationModel.xml  
 (replace adziki with your hostname)
 
 
 Now, go back to VS .NET, and run the Scenario Controller project (with Program.cs as
 the start class).
 
 What it does:
 The ScenCon sends events to the Server Stub, as the Server receives
 the events, it puts output on the command prompt's console.  For this
 test, it should tick 5 times, create 2 new objects, tick 5 more times
 and then create 2 Move Events on the two previously created objects.
 At this point, the EventQueue is empty, so it runs until you Ctrl+C on
 the command prompt.
 
 Upcoming:
 Once Dennis handles XML parsing, we'll be able to grab scenario data from a scenario
 file, plus we need to get event listener looking for move complete events, and we
 need to implement a state manager.