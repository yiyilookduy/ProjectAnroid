package connection;

import android.content.Intent;
import android.os.AsyncTask;
import android.util.Log;

import org.jetbrains.annotations.NotNull;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.MultipartBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import utils.JSONManipulator;

public class MyConnection {
    private OkHttpClient okHttpClient = new OkHttpClient();
    JSONManipulator jsm = new JSONManipulator();

    public String Login(String username, String password) throws IOException{
        RequestBody formBody = new FormBody.Builder()
                .add("username", username)
                .add("password", password)
                .build();

        Request request = new Request.Builder()
                .url("http://171.245.197.16:8080/Home/Login")
                .post(formBody)
                .build();

        Response response = okHttpClient.newCall(request).execute();
        return response.body().string();
    }
}
