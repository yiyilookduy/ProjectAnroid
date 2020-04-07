package com.example.groupproject;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;

public class teacherActivity extends AppCompatActivity {

    @Override
    protected void onRestart() {
        super.onRestart();
        finish();
        startActivity(getIntent());
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_teacher_page);
        setTitle("Options");

//        ImageView imgManageAttendance = findViewById(R.id.imgManageAttendance);
        ImageView imgReviewTicket = findViewById(R.id.imgReviewTicket);

//        imgManageAttendance.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(teacherActivity.this, teacherManageAttendanceActivity.class);
//                String username = getIntent().getStringExtra("username");
//                intent.putExtra("username",username);
//                startActivity(intent);
//            }
//        });

        imgReviewTicket.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(teacherActivity.this, teacherReviewTicketActivity.class);
                String username = getIntent().getStringExtra("username");
                intent.putExtra("username",username);
                startActivity(intent);
            }
        });
    }
}
