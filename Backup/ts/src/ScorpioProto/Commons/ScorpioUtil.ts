export class ScorpioUtil {
    public static ToString(list:Array<any>):string {
        if (list == null || list.length == 0) { return "[]" }
        let length = list.length
        let str = "["
        for (let i = 0; i < length; ++i) {
            str += list[i]
            if (i != length - 1) {
                str += ","
            }
        }
        str += "["
        return str
    }
}