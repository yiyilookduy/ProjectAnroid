package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.SimpleAdapter;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import com.example.groupproject.R.id;

import dto.Class_Subject;

public class studentCheckAttendanceActivity extends AppCompatActivity {
    String username;
    List<Map<String,String>> studentAttendanceData;
    SimpleAdapter simpleAdapter;
    Class_Subject classSubject = new Class_Subject("1","swd","Monday",1);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_check_attendance);
        setTitle("Check Student Attendance");
        username = getIntent().getStringExtra("username");
        studentAttendanceData = new ArrayList<>();
    }

    class getStudentSubjectStudy extends AsyncTask<String,String,String>{

        @Override
        protected String doInBackground(String... strings) {
            return null;
        }
    }

    class getClassSubjectSchedule extends AsyncTask<String,String,String>{

        @Override
        protected String doInBackground(String... strings) {
            return null;
        }
    }

    private void readJSON(String s){
        //read Json
        try {
            JSONObject jsonObject = new JSONObject(s);
            String classDataInfo = jsonObject.getString("Data");
            Log.i("user Data",classDataInfo);
            JSONArray array1 = new JSONArray(classDataInfo);
            for(int i =0;i<array1.length();i++){
                JSONObject jsonPart = array1.getJSONObject(i);

            }
        }catch (Exception e){
            e.printStackTrace();
        }
    }
}
