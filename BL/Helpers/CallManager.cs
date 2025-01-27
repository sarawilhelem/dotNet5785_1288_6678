

using DalApi;

namespace Helpers;

internal class CallManager
{
    private static IDal s_dal = Factory.Get;
    public IEnumerable<BO.CallInList> ReadAll()
    {
        s_dal.Call.ReadAll();
    }
}
