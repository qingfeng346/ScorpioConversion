//本文件为自动生成，请不要手动修改
package scov;
public enum TestEnum {
    Test1(1),
    Test2(2),
    Test3(3),
    value1(4),
    value2(5),
    ;
    private final int value;
    private TestEnum(int value) { this.value = value; }
    public final int getValue() { return this.value; }
    public static TestEnum valueOf(int value) {
        switch (value) {
        case 1: return Test1;
        case 2: return Test2;
        case 3: return Test3;
        case 4: return value1;
        case 5: return value2;
        default: return null;
        }
    }
    public static TestEnum stringOf(String value) {
        switch (value) {
        case "Test1": return Test1;
        case "Test2": return Test2;
        case "Test3": return Test3;
        case "value1": return value1;
        case "value2": return value2;
        default: return null;
        }
    }
}