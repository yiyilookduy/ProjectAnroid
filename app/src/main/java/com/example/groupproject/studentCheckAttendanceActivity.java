package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.InputStream;
import com.example.groupproject.R.id;

public class studentCheckAttendanceActivity extends AppCompatActivity {
    private String userData = "";
    private String classID = "";
    private String subjectIDJson="";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_check_attendance);
        setTitle("Check Student Attendance");
        readFile("3subjects.json");
        readJSON(userData);
        TextView s1m = (TextView) findViewById(id.S1MON);
        s1m.setText("");

    }

    private void readFile(String fileName){
        try{
            InputStream is = getAssets().open(fileName);
            int size = is.available();
            byte[] buffer = new byte[size];
            is.read(buffer);
            is.close();
            userData = new String(buffer);
        }catch (Exception e){
            e.printStackTrace();
        }
    }

    private void readJSON(String s){
        //read Json
        try {
            JSONObject jsonObject = new JSONObject(userData);
            String classDataInfo = jsonObject.getString("Data");
            Log.i("user Data",classDataInfo);
            JSONArray array1 = new JSONArray(classDataInfo);
            for(int i =0;i<array1.length();i++){
                JSONObject jsonPart = array1.getJSONObject(i);
                subjectIDJson=jsonPart.getString("subjectId");
            }
        }catch (Exception e){
            e.printStackTrace();
        }
    }
}
