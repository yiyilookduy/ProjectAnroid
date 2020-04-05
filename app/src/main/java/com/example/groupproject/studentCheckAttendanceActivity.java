package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.annotation.SuppressLint;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
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
    TextView txtS1M, txtS2M, txtS3M,txtS4M,txtS5M,txtS6M,txtS7M,txtS8M
            ,txtS1T,txtS2T,txtS3T,txtS4T,txtS5T,txtS6T,txtS7T,txtS8T
            ,txtS1W, txtS2W, txtS3W,txtS4W,txtS5W,txtS6W,txtS7W,txtS8W
            ,txtS1THUR, txtS2THUR, txtS3THUR,txtS4THUR,txtS5THUR,txtS6THUR,txtS7THUR,txtS8THUR
            ,txtS1F, txtS2F, txtS3F,txtS4F,txtS5F,txtS6F,txtS7F,txtS8F
            ,txtS1S, txtS2S, txtS3S,txtS4S,txtS5S,txtS6S,txtS7S,txtS8S;
    Spinner weekSelector;
    String selectedDate;

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
        final ArrayList<String> date = new ArrayList<String>();
        date.add("03/16/2020-03/22/2020");
        date.add("03/23/2020-03/29/2020");
        date.add("03/30/2020-04/05/2020");
        ArrayAdapter arrayAdapter = new ArrayAdapter(this,android.R.layout.simple_spinner_item,date);
        arrayAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        weekSelector.setAdapter(arrayAdapter);

        weekSelector.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                try {
                    clearTextData();
                    String text = weekSelector.getSelectedItem().toString().substring(0,10).replaceAll("/","%2F");
                    String attendanceDataOfSpecificWeek = new getStudentAttendanceDataUrl().execute("http://171.245.197.16:8080/Attendance/GetScheduleOnWeek?studentId="+username+"&date="+text).get();
                    readStudentAttendanceJSONData(attendanceDataOfSpecificWeek);
                    setTextData();
                    clearDayArray();
                } catch (ExecutionException | InterruptedException e) {
                    e.printStackTrace();
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

    }
    private void clearDayArray(){
        try {
            Mon.clear();
            Tue.clear();
            Wed.clear();
            Thu.clear();
            Fri.clear();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    //Turn Boolean attendance data to attended and absent
    private String translateAttendance(Boolean b){
        String attendance;
        if(b==true){
            attendance = "(attended)";
        }else{
            attendance = "(absent)";
        }
        return attendance;
    }

    //Set text data for text in table
    private void setTextData(){
        try {
            //Monday
            txtS1M.setText(Mon.get(0).getSubjectName()+"\n"+ translateAttendance(Mon.get(0).getAttendance()));
            txtS2M.setText(Mon.get(1).getSubjectName()+"\n"+ translateAttendance(Mon.get(1).getAttendance()));
            txtS3M.setText(Mon.get(2).getSubjectName()+"\n"+ translateAttendance(Mon.get(2).getAttendance()));
            txtS4M.setText(Mon.get(3).getSubjectName()+"\n"+ translateAttendance(Mon.get(3).getAttendance()));
            txtS5M.setText(Mon.get(4).getSubjectName()+"\n"+ translateAttendance(Mon.get(4).getAttendance()));
            txtS6M.setText(Mon.get(5).getSubjectName()+"\n"+ translateAttendance(Mon.get(5).getAttendance()));
            txtS7M.setText(Mon.get(6).getSubjectName()+"\n"+ translateAttendance(Mon.get(6).getAttendance()));
            txtS8M.setText(Mon.get(7).getSubjectName()+"\n"+ translateAttendance(Mon.get(7).getAttendance()));
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Tuesday
            txtS1T.setText(Tue.get(0).getSubjectName()+"\n"+ translateAttendance(Tue.get(0).getAttendance()));
            txtS2T.setText(Tue.get(1).getSubjectName()+"\n"+ translateAttendance(Tue.get(1).getAttendance()));
            txtS3T.setText(Tue.get(2).getSubjectName()+"\n"+ translateAttendance(Tue.get(2).getAttendance()));
            txtS4T.setText(Tue.get(3).getSubjectName()+"\n"+ translateAttendance(Tue.get(3).getAttendance()));
            txtS5T.setText(Tue.get(4).getSubjectName()+"\n"+ translateAttendance(Tue.get(4).getAttendance()));
            txtS6T.setText(Tue.get(5).getSubjectName()+"\n"+ translateAttendance(Tue.get(5).getAttendance()));
            txtS7T.setText(Tue.get(6).getSubjectName()+"\n"+ translateAttendance(Tue.get(6).getAttendance()));
            txtS8T.setText(Tue.get(7).getSubjectName()+"\n"+ translateAttendance(Tue.get(7).getAttendance()));
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Wednesday
            txtS1W.setText(Wed.get(0).getSubjectName()+"\n"+ translateAttendance(Wed.get(0).getAttendance()));
            txtS2W.setText(Wed.get(1).getSubjectName()+"\n"+ translateAttendance(Wed.get(1).getAttendance()));
            txtS3W.setText(Wed.get(2).getSubjectName()+"\n"+ translateAttendance(Wed.get(2).getAttendance()));
            txtS4W.setText(Wed.get(3).getSubjectName()+"\n"+ translateAttendance(Wed.get(3).getAttendance()));
            txtS5W.setText(Wed.get(4).getSubjectName()+"\n"+ translateAttendance(Wed.get(4).getAttendance()));
            txtS6W.setText(Wed.get(5).getSubjectName()+"\n"+ translateAttendance(Wed.get(5).getAttendance()));
            txtS7W.setText(Wed.get(6).getSubjectName()+"\n"+ translateAttendance(Wed.get(6).getAttendance()));
            txtS8W.setText(Wed.get(7).getSubjectName()+"\n"+ translateAttendance(Wed.get(7).getAttendance()));
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Thursday
            txtS1THUR.setText(Thu.get(0).getSubjectName()+"\n"+ translateAttendance(Thu.get(0).getAttendance()));
            txtS2THUR.setText(Thu.get(1).getSubjectName()+"\n"+ translateAttendance(Thu.get(1).getAttendance()));
            txtS3THUR.setText(Thu.get(2).getSubjectName()+"\n"+ translateAttendance(Thu.get(2).getAttendance()));
            txtS4THUR.setText(Thu.get(3).getSubjectName()+"\n"+ translateAttendance(Thu.get(3).getAttendance()));
            txtS5THUR.setText(Thu.get(4).getSubjectName()+"\n"+ translateAttendance(Thu.get(4).getAttendance()));
            txtS6THUR.setText(Thu.get(5).getSubjectName()+"\n"+ translateAttendance(Thu.get(5).getAttendance()));
            txtS7THUR.setText(Thu.get(6).getSubjectName()+"\n"+ translateAttendance(Thu.get(6).getAttendance()));
            txtS8THUR.setText(Thu.get(7).getSubjectName()+"\n"+ translateAttendance(Thu.get(7).getAttendance()));
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Friday
            txtS1F.setText(Fri.get(0).getSubjectName()+"\n"+ translateAttendance(Fri.get(0).getAttendance()));
            txtS2F.setText(Fri.get(1).getSubjectName()+"\n"+ translateAttendance(Fri.get(1).getAttendance()));
            txtS3F.setText(Fri.get(2).getSubjectName()+"\n"+ translateAttendance(Fri.get(2).getAttendance()));
            txtS4F.setText(Fri.get(3).getSubjectName()+"\n"+ translateAttendance(Fri.get(3).getAttendance()));
            txtS5F.setText(Fri.get(4).getSubjectName()+"\n"+ translateAttendance(Fri.get(4).getAttendance()));
            txtS6F.setText(Fri.get(5).getSubjectName()+"\n"+ translateAttendance(Fri.get(5).getAttendance()));
            txtS7F.setText(Fri.get(6).getSubjectName()+"\n"+ translateAttendance(Fri.get(6).getAttendance()));
            txtS8F.setText(Fri.get(7).getSubjectName()+"\n"+ translateAttendance(Fri.get(7).getAttendance()));
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void clearTextData(){
        try {
            //Monday
            txtS1M.setText("");
            txtS2M.setText("");
            txtS3M.setText("");
            txtS4M.setText("");
            txtS5M.setText("");
            txtS6M.setText("");
            txtS7M.setText("");
            txtS8M.setText("");
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Tuesday
            txtS1T.setText("");
            txtS2T.setText("");
            txtS3T.setText("");
            txtS4T.setText("");
            txtS5T.setText("");
            txtS6T.setText("");
            txtS7T.setText("");
            txtS8T.setText("");
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Wednesday
            txtS1W.setText("");
            txtS2W.setText("");
            txtS3W.setText("");
            txtS4W.setText("");
            txtS5W.setText("");
            txtS6W.setText("");
            txtS7W.setText("");
            txtS8W.setText("");
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Thursday
            txtS1THUR.setText("");
            txtS2THUR.setText("");
            txtS3THUR.setText("");
            txtS4THUR.setText("");
            txtS5THUR.setText("");
            txtS6THUR.setText("");
            txtS7THUR.setText("");
            txtS8THUR.setText("");
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            //Friday
            txtS1F.setText("");
            txtS2F.setText("");
            txtS3F.setText("");
            txtS4F.setText("");
            txtS5F.setText("");
            txtS6F.setText("");
            txtS7F.setText("");
            txtS8F.setText("");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    //find all View by Id
    private void callViewsById(){
        weekSelector = findViewById(R.id.spnWeekSelector);
        //Monday
        txtS1M = findViewById(R.id.S1MON);
        txtS2M = findViewById(R.id.S2MON);
        txtS3M = findViewById(R.id.S3MON);
        txtS4M = findViewById(R.id.S4MON);
        txtS5M = findViewById(R.id.S5MON);
        txtS6M = findViewById(R.id.S6MON);
        txtS7M = findViewById(R.id.S7MON);
        txtS8M = findViewById(R.id.S8MON);
        //Tuesday
        txtS1T = findViewById(R.id.S1TUE);
        txtS2T = findViewById(R.id.S2TUE);
        txtS3T = findViewById(R.id.S3TUE);
        txtS4T = findViewById(R.id.S4TUE);
        txtS5T = findViewById(R.id.S5TUE);
        txtS6T = findViewById(R.id.S6TUE);
        txtS7T = findViewById(R.id.S7TUE);
        txtS8T = findViewById(R.id.S8TUE);
        //Wednesday
        txtS1W = findViewById(R.id.S1WED);
        txtS2W = findViewById(R.id.S2WED);
        txtS3W = findViewById(R.id.S3WED);
        txtS4W = findViewById(R.id.S4WED);
        txtS5W = findViewById(R.id.S5WED);
        txtS6W = findViewById(R.id.S6WED);
        txtS7W = findViewById(R.id.S7WED);
        txtS8W = findViewById(R.id.S8WED);
        //Thursday
        txtS1THUR = findViewById(R.id.S1THU);
        txtS2THUR = findViewById(R.id.S2THU);
        txtS3THUR = findViewById(R.id.S3THU);
        txtS4THUR = findViewById(R.id.S4THU);
        txtS5THUR = findViewById(R.id.S5THU);
        txtS6THUR = findViewById(R.id.S6THU);
        txtS7THUR = findViewById(R.id.S7THU);
        txtS8THUR = findViewById(R.id.S8THU);
        //Friday
        txtS1F = findViewById(R.id.S1FRI);
        txtS2F = findViewById(R.id.S2FRI);
        txtS3F = findViewById(R.id.S3FRI);
        txtS4F = findViewById(R.id.S4FRI);
        txtS5F = findViewById(R.id.S5FRI);
        txtS6F = findViewById(R.id.S6FRI);
        txtS7F = findViewById(R.id.S7FRI);
        txtS8F = findViewById(R.id.S8FRI);
        //Saturday
        txtS1S = findViewById(R.id.S1SAT);
        txtS2S = findViewById(R.id.S2SAT);
        txtS3S = findViewById(R.id.S3SAT);
        txtS4S = findViewById(R.id.S4SAT);
        txtS5S = findViewById(R.id.S5SAT);
        txtS6S = findViewById(R.id.S6SAT);
        txtS7S = findViewById(R.id.S7SAT);
        txtS8S = findViewById(R.id.S8SAT);
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

    //Read all data in Json file and transfer data to object and put object to Day list
    private void readStudentAttendanceJSONData(String s){
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
