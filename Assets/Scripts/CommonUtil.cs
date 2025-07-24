using UnityEngine;

public class CommonUtil
{
    // 문자열 → 안정적 해시값 (간단하게 DJB2 방식 사용)
    public static int StableHash(string s)
    {
        unchecked
        {
            int hash = 5381;
            foreach (char c in s)
                hash = ((hash << 5) + hash) + c; // hash * 33 + c
            return Mathf.Abs(hash);
        }
    }
}
