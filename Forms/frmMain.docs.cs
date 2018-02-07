using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyReplace
{
    /// <summary>
    /// -deto, entangle, conc/sonic, and also a enter/tab disable
    /// - Consider a failure as a partial success instead. -- ALL FAILURES ARE PARTIAL SUCCESSES?
    /// - System.Diagnostics.Debug.WriteLine  is a thing?????!?!??
    /// logo courtesy https://us.123rf.com/450wm/vivat191192/vivat1911921508/vivat191192150800117/43826042-arrow-circle-icon-cycle-signs-colored.jpg
    /// 
    /// -> the key timelogger utility! uses keyproc hook too
    /// 
    /// -> the key display utility! displays a keyboard and hardware keys light up in red while injected keys light up in blue.
    /// 
    /// [x] osd done 2018-01-02 with very NINJA window following support.
    ///     - fixed to delay the input if its too fast, the queue stacks up
    ///     - reset the prevEvt timer only if the key was actually passed through
    /// 
    /// [x] Hooker - done 2017 10 19 @ 12:30 am
    /// [x] Macro compilation - testing 2017 10 20 @ 7:07 pm
    /// [x] Input Sender
    /// [X] GUI for managing key strokes mappings - done october 22 @ 12:38 am
    /// [X] Add original key- done october 22 @ 1:32 am
    /// [X] Up/Down button for re-arranging input events
    /// [X] Implement Options klaas - done october 22 @ 3:43 pm
    /// [X] Save load Macro and settings. - complete 2017-10-23 @ 7:00 pm
    /// [X] Startup/Shutdown / Tray Icon - done october 22 @ 6:37 pm - 8:39 pm
    /// [x] single instanced - https://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-application
    /// [X] Macro Page Table methodology - save load edit compile display -- page tables.
    ///     -- Completed on October 24, 2017 @ 10:56 PM. GREAT SUCCESS!
    ///     able to save/load/edit/compile/display as well as switch between diff
    ///     macro pages live.
    /// [X] Prevent selection of current page when adding new or editing a macro. - done oct25 2017 @ 12:33 AM -- using linq where(item,idx) delegate.
    /// [ ] Middle mouse button or something. ---
    ///     -- specify left middle or right mouse buttons as triggers. 
    ///     -- can left/middle/right click (with mod keys) on the box to specify that type of input. a note will be left so.
    ///     -- the note will be visible if IsInput is FALSE. IsInput meaning IsInjectedInput, not IsUserInput that triggers the events chain

    /// [X] A macro with no events should block the input? - it will block the input now. 2017-10-25 @ 1:28 am.
    /// [ ] Option to hide to tray or close - when exiting via close button
    /// [X] menu strip for export/import macros, and also for file->exit>?  complete 2017-11-03 @ 10:57 PM
    /// [X] mouse support for input - nov 3 2017 @ 11:08p
    /// [X] specific process targetting - nov 3 2017 @ 11:08p
    /// [X] new filesaveload method using serializable - nov 3 2017 @ 11:08p
    /// [X] key hook on new thread, different from mouse hook  - nov 3 2017 @ 11:08p
    /// [x] user can type input right away when opening the new input/trigger form. nov 5 2017 @ 1:23 AM
    /// 
    /// [x] ensure edited item is selected and visible after edit - nov 5 2017 @ 7:34PM
    /// 
    /// [ ] display process status - not found, no permission, more than one found, or found. on the process window subregion selection.
    /// 
    /// [ ] combine some of logic for up/down keys
    /// 
    /// [ ] Color activation (per-macro basis)
    /// 
    /// [ ] Could set "show in taskbar" to false and have the close button display warning to close program.
    /// 
    /// 
    /// [  ] Options interface. (add here a checkmark to indicate rs window foreground. ) (maybe move checkboxes in too)
    /// [X] Framework class for options complete. adding shit should be simple now. default values are initialized statically. oct 25 2017 @ 4.42pm.
    /// [ ] Disable-All hotkey : maybe only activate if scrllock/capslock/numslock is ON?
    /// [ ] Activate only if rs window in foreground option
    /// [ ] Sleep delay toggling - ie between injected keystrokes as they are processed by keyproc.
    /// [X] filter "up" input events if the key is already up! can use IsKeyPushedDown(vk). have this as an option too. oct 25 2017 @ 4:42 pm.
    /// [ ] OSD for macro status - click through with options!
    /// 
    /// [ ] Show duplicate triggers accross multiple macros on main screen - can highlight specific cells to indicate this
    ///     possible to have tooltip too? and icon? 
    ///     
    /// [ ] show messages on tray icon on first install, etc
    /// [ ] macro max activation delays?
    /// [ ] save listview columns sizes?
    /// [ ] feedback upon deleting or adding macros, for main page and macro editor page - could change a label text
    /// 
    /// [ ] if compiling a macro for a mouse trigger - REMOVE the original key, since it sends a shit key stroke ?
    /// 
    /// [ ] "[p]" in front of macros that pass original input?
    /// 
    /// [ ] being able to copy macros - we can move macro pages up/down already.
    /// note that cutting does not insta save. this could lead  to problems, when cutting and reordering an item.
    /// 
    /// [X] DOUBLE click item in listview to edit it. same thing, be able to edit triggers and such. - completed 2017 nov 05 @ 1:58 AM
    /// 
    /// [ ] combine add/rename/delete macro page into one submenu. the code does not need to change for this.
    ///     then we add a 'edit' menu for 
    /// [X] if main form has focus, we must disable macros!
    /// [ ] a way to clear triggers/input events on the new/edit macro dialog.
    /// [ ] multiselect on the trigger/event lists on new macro dialog, and maybe arrange the items, especially triggers, to not be all in one line.
    ///  ^could use listview and this would allow for ez multiselect.
    /// [ ] use delete hotkey to delete things - prompt if deleting empty page too?
    ///  
    /// [X] import export current page or all pages into a file. can have sep file formats for single page or page collection?
    ///     ^kinda problematic because macros with table switches reference other tables by NAME. even with index references, this is
    ///     not quite feasible.
    /// 
    /// [ ] 'global' macro page whos macros are effective accross all pages.
    /// [ ] reorder macros on main page, ideally via drag?
    /// 
    /// [ ] tabview for diff macro pages, instead of a combobox? slow if many pages as well.
    /// [ ] (orig) -> original?
    /// [X] No events. (switch to deto) -> Switch to deto --- if there is no events just add an extra if statement, very easy
    ///     ^completed 2017 nov 03
    /// [ ] visual feedback - "macros updated" on edit macro etc.
    /// 
    /// [ ] implement .HasProcessName? well we sort of consider the screen as a "process" so every
    /// trigger always DOES have a process name...
    /// 
    /// Bug
    /// [ ] notify icon balloon dont show.. make it show when minimized etc so ppl know its a thing.
    /// [X] Does not distinguish between LeftShift and Right shift when computing modifier events..
    ///     This is gonna be difficult to correct at runtime. unless we distinguish if its LShift + key or whatever.
    ///         ^Modifier events no longer computed as of 2017 nov 05 @ 9:03 PM.
    /// [x] Does not supress the next key up event, or the next keydown event properly - fixed 2017-10-22 @ 11:57 pm
    /// [ ] frmNewKey shows LineFeed when shown again after selecting "original key" and accepting.
    /// found the cause - setKeyState(false, false, false, 10) - where 10 is LineFeed.
    /// 
    /// [X] Supress the additional repeats if a macro was executed on that key? - done 2017 oct 22 @ 5:57pm
    /// [X] macro triggered by key up, and ends with original key down. do we supress the next downkey on repeats.
    ///     -if we find a macro to execute, and if the keydown has a macro, it would be supressed anyway.
    ///     -if the keyup macro ends with the original key in the down state, that is the fault of the user anyway.
    ///     -we don't attempt to regulate that
    ///     -investigated and patched 2017 10 23 @ 12:44AM
    /// [X] Fix modifier key up / key down correctness
    ///           ^Modifier events no longer computed as of 2017 nov 05 @ 9:03 PM.
    /// [x] Used approach to delay injected keys on their way in. this approach can fuck up other
    ///     macro programs such as AHK. but hey it works if you only delay the injected downKeys
    ///     - we used a box-muller transform code from stackexchange. it is 2 layers of box-muller lol
    /// [ ] make input key/text forms not disappear when clicking to another application and back on the form.
    /// 
    /// 
    /// 
    /// [x] every time u save the settings, its guna copy and replace the startup executable if startup is checked.
    /// u need to make sure u check if the startup executable is the right version before copypasta shit, do so on startup too etc
    /// -- FIXED by splitting into 2 seperate methods for startup and all other settings. oct 25 2017 @ 4:41 pm.
    /// 
    /// [ ] add settings check, ie if bool != "True" and != "False", in case tampering happens to the registry.
    /// can use default values.
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    ///interesting methodology -- 
    /// [ ]Only use scancodes for everything, so we distinguish L+R keys, and keys that map to the same thing
    /// ..and when we go to input, we use translate key or some shit?
    /// [ ] maybe we can use enum for LShift+RShift.  The RShift is gonna be LShift | another bit, so that to check if shift
    ///     is down AT ALL, we can just check for LShift.
    /// 
    /// [ ] Possible to edit the hookstruct and set the injected flag to false?
    /// 
    /// Store the regular user's keystroke speeds. Then just replay them.
    /// If the user types faster than what was allowed, we just suppress the input and place them in a buffer,
    /// noting the times at which they are received.
    /// 
    /// We allow our own keystrokes to play, then send the user's keystrokes, within their timely fashion.
    /// If the user presses MORE keys, we simply place them once again in the buffer. interesting..
    /// 
    /// 
    /// Notes - 
    /// 
    /// There is a HELP message that is sent along with the WM_KEYDOWN on f1. NOT ANYMORE
    /// Also there is a WM_CHAR message that is sent as well, inbetween the WM_KEYDOWN and WM_KEYUP.
    /// ^The WM_CHAR is sent by itself sorta..
    /// The Scan code is always 00 for injected keys, but if this is an issue, 
    /// then so would the Injected flag be.. and we love the injected flag!
    /// 
    /// If repeating keys, then there is a bit of an issue
    /// 
    /// Okay so the situation is, using KEYBOARD gives you the repeat count, but not the isInjected flag..
    /// But KEYBOARD_LL will give you the isInjected flag, but not the repeat count... ffs...
    /// 
    /// We can keep ourselves from processing our already injected keys, using the LL method.
    /// Now all we have to do is sort out the repetition problem.
    /// We can keep an array of keyboard keys that we choose to block.
    /// We need to update this array with GetAsyncKeyState perhaps, occasionally?
    /// 
    /// Note - 
    /// This program only works when the runescape client is the foreground. (get foreground executable name)
    /// if running runescape client as administrator, run this program as administrator too - otherwise
    /// it will not be able to replace keys.
    /// 
    /// KeyGroup to KeySequence mapping.
    /// Any key in KeyGroup will map to a sequence of keys.
    /// 
    /// Perhaps we initialize array of length of keys.
    /// 
    /// Input the sequence into the correct spots for quicker lookup.
    /// 
    /// On intercept key - 
    /// 
    /// set the 'current sequence' to the sequence of keys first, before sending input.
    /// ^but before that, check against current sequence.
    /// 
    /// NOTE ---- Controls are hidden beyond the right border of the main form.
    /// 
    /// When sending input --
    /// collect data on keystrokes
    /// - keep array downLength of keys count length.
    /// on key down set downLength[vk] to current date
    /// on key up, do DateTime.Now - downLength[vk], if vk is not null. Otherwise i guess we just ignore it. (will happen if alt key.)
    /// for nextkeydelay, just keep track of the last time a key was down. any key will do.
    /// 
    /// genhist
    /// - iterate through every byte, and pick a bin size and increment the bin counter (5ms apart?)
    /// - then we can just make a row of a csv for every bin.
    /// 
    /// </summary>
    public partial class frmMain
    {
        public frmMain()
        {
            InitializeComponent();
            //More initialization in  frmMain_Load.

            /* example kode
            Trigger XY = new Trigger(KeyTrigger.KeyDown, (int)Keys.Q, ModifierKey.None);

            Macro magie = new Macro("macro1");
            magie.AddTrigger(XY);
            //magie.AddInputEvent(new InputEvent(InputEventType.KeyPress, (int)SpecialKey.OriginalKey, ModifierKey.None));
            magie.AddInputEvent(new InputEvent(InputEventType.KeyDown, (int)Keys.A, ModifierKey.None));
            magie.AddInputEvent(new InputEvent(InputEventType.KeyDown, (int)Keys.B, ModifierKey.None));



            Trigger XY2 = new Trigger(KeyTrigger.KeyUp, (int)Keys.Q, ModifierKey.None);

            Macro magie2 = new Macro("macro2");
            magie2.AddTrigger(XY2);
            magie2.AddInputEvent(new InputEvent(InputEventType.KeyUp, (int)Keys.A, ModifierKey.None));
            magie2.AddInputEvent(new InputEvent(InputEventType.KeyUp, (int)Keys.B, ModifierKey.None));

            macro_table.Add(XY, magie.Compile(XY));
            macro_table.Add(XY2, magie2.Compile(XY2));
            */
        }
    }
}
