package test;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;

import ScorpioProto.Commons.ScorpioReader;
import scov.*;

public class launch {
	public static void main(String[] args) {
		ByteArrayOutputStream output = new ByteArrayOutputStream();
    	try {
    		FileInputStream stream = new FileInputStream(new File("../Test.data"));
            int n = 0;
            byte[] buffer = new byte[4096];
            while (-1 != (n = stream.read(buffer))) {
                output.write(buffer, 0, n);
            }
            stream.close();
            TableTest t = new TableTest();
			t.Initialize("wwww", new ScorpioReader(output.toByteArray()));
			int a = 0;
    	} catch (Exception e) {
    		
    	}
    	System.out.println("222222222222222222");
		// TODO Auto-generated method stub
		
	}
}
