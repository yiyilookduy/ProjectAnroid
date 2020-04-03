package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.Spinner;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;

import dto.Class_Subject;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

public class studentCheckAttendanceActivity extends AppCompatActivity {
    String username,subject,day;
    int slot;
    Boolean attendance;
    List<Class_Subject> Mon,Tue,Wed,Thu,Fri;
    Class_Subject classSubject;
    TextView txtS1M, txtS2M, txtS3M,txtS1T,txtS2T,txtS3T;
    Spinner weekSelector;

    @Override
    protected void onRestart() {
        super.onRestart();
        finish();
        startActivity(getIntent());
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_student_check_attendance);
        setTitle("Check Student Attendance");
        username = getIntent().getStringExtra("username");
        Mon = new ArrayList<>();
        Tue = new ArrayList<>();
        callViewsById();
        try {
            String attendanceDataOfSpecificWeek = new getStudentAttendanceDataUrl().execute("http://171.245.197.16:8080/Attendance/GetScheduleOnWeek?studentId="+username+"&date=03%2F21%2F2020").get();
            readStudentAttendanceJSONData(attendanceDataOfSpecificWeek);
            txtS1M.setText(Mon.get(0).getSubjectName()+"\n"+ translateAttendance(Mon.get(0).getAttendance()));
            txtS2M.setText(Mon.get(1).getSubjectName()+"\n"+ translateAttendance(Mon.get(1).getAttendance()));
            txtS3M.setText(Mon.get(2).getSubjectName()+"\n"+ translateAttendance(Mon.get(2).getAttendance()));
            txtS1T.setText(Tue.get(0).getSubjectName()+"\n"+ translateAttendance(Tue.get(0).getAttendance()));
            txtS2T.setText(Tue.get(1).getSubjectName()+"\n"+ translateAttendance(Tue.get(1).getAttendance()));
            txtS3T.setText(Tue.get(2).getSubjectName()+"\n"+ translateAttendance(Tue.get(2).getAttendance()));
        } catch (ExecutionException | InterruptedException e) {
            e.printStackTrace();
        }
    }

    private String translateAttendance(Boolean b){
        String attendance;
        if(b==true){
            attendance = "(attend)";
        }else{
            attendance = "(absent)";
        }
        return attendance;
    }

    private void callViewsById(){
        weekSelector = findViewById(R.id.spnWeekSelector);
        txtS1M = findViewById(R.id.S1MON);
        txtS2M = findViewById(R.id.S2MON);
        txtS3M = findViewById(R.id.S3MON);
        txtS1T = findViewById(R.id.S1TUE);
        txtS2T = findViewById(R.id.S2TUE);
        txtS3T = findViewById(R.id.S3TUE);
    }

    private class getStudentAttendanceDataUrl extends AsyncTask<String,String,String>{
        OkHttpClient okHttpClient = new OkHttpClient.Builder()
                .connectTimeout(15, TimeUnit.SECONDS)
                .writeTimeout(15, TimeUnit.SECONDS)
                .readTimeout(15, TimeUnit.SECONDS)
                .retryOnConnectionFailure(true)
                .build();

        @Override
        protected String doInBackground(String... strings) {
            Request.Builder builder = new Request.Builder();
            builder.url(strings[0]);
            Request request = builder.build();

            try {
                Response response = okHttpClient.newCall(request).execute();
                return response.body().string();
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }

        @Override
        public void onPostExecute(String s) {
            super.onPostExecute(s);
        }
    }

    private void readStudentAttendanceJSONData(String s){
        //read Json
        try {
            JSONObject JsonData = new JSONObject(s);
            String classAttendanceDataInfo = JsonData.getString("Data");
            JSONObject JsonDaySlot = new JSONObject(classAttendanceDataInfo);

            for(int i = 1;i<=8;i++){
                try {
                    String M = JsonDaySlot.getString("Monday/"+i);
                    M = "["+M+"]";
                    JSONArray MArray = new JSONArray(M);
                    for(int i1 = 0;i1<MArray.length();i1++){
                        JSONObject jsonPart = MArray.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Monday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Mon.add(classSubject);
                    }

                    String T1 = JsonDaySlot.getString("Tuesday/"+i);
                    T1 = "["+T1+"]";
                    JSONArray T1Array = new JSONArray(T1);
                    for(int i1 = 0;i1<T1Array.length();i1++){
                        JSONObject jsonPart = T1Array.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Tuesday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Tue.add(classSubject);
                    }

                    String W1 = JsonDaySlot.getString("Wednesday/"+i);
                    W1 = "["+W1+"]";
                    JSONArray W1Array = new JSONArray(W1);
                    for(int i1 = 0;i1<W1Array.length();i1++){
                        JSONObject jsonPart = W1Array.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Wednesday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Wed.add(classSubject);
                    }

                    String Thursday = JsonDaySlot.getString("Thursday/"+i);
                    Thursday = "["+Thursday+"]";
                    JSONArray ThuArray = new JSONArray(Thursday);
                    for(int i1 = 0;i1<ThuArray.length();i1++){
                        JSONObject jsonPart = ThuArray.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Thursday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Thu.add(classSubject);
                    }

                    String Friday = JsonDaySlot.getString("Friday/"+i);
                    Friday = "["+Friday+"]";
                    JSONArray friArray = new JSONArray(Friday);
                    for(int i1 = 0;i1<friArray.length();i1++){
                        JSONObject jsonPart = friArray.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Friday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Fri.add(classSubject);
                    }

                    String Saturday = JsonDaySlot.getString("Saturday/"+i);
                    Saturday = "["+Saturday+"]";
                    JSONArray satArray = new JSONArray(Saturday);
                    for(int i1 = 0;i1<satArray.length();i1++){
                        JSONObject jsonPart = satArray.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Saturday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Fri.add(classSubject);
                    }

                    String Sunday = JsonDaySlot.getString("Sunday/"+i);
                    Sunday = "["+Sunday+"]";
                    JSONArray sunArray = new JSONArray(Sunday);
                    for(int i1 = 0;i1<sunArray.length();i1++){
                        JSONObject jsonPart = sunArray.getJSONObject(i1);
                        subject = jsonPart.getString("subject");
                        slot = i1;
                        day = "Sunday";
                        attendance = jsonPart.getBoolean("atten");
                        classSubject = new Class_Subject(subject,day,attendance,slot);
                        Fri.add(classSubject);
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }catch (Exception e){
            e.printStackTrace();
        }
    }
}
