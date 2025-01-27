using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// Details of a closed call in the list
/// </summary>
/// <param name="Id"> Id of specific call</param>
/// <param name="CallType">Type of call:  Take_Care_At_Home, Take_Care_Out, Physiotherapy</param>
/// <param name="Address">from where the call was accepted</param>
/// <param name="OpenCallTime">the time when the call was accepted</param>
/// <param name="StartCallTime">the time when a volunteer takes the call</param>
/// <param name="FinishCallTime">the time when a volunteer updates that he finished the process</param>
/// <param name="Finish_Type">Type of finish:   Addressed,SelfCancel,ManageCancel,Expired</param>

public class ClosedCallInList
{
    public int Id {  get; init; }
    public Call_Type CallType { get; set; }
    public string Address {  get; set; }
    public DateTime OpenCallTime { get; set; }
    public DateTime StartCallTime { get; set; }
    public DateTime? FinishCallTime { get; set; }
    public Finish_Type? Finish_Type {  get; set; } 

}
