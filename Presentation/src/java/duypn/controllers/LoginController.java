/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package duypn.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import duypn.classes.ApplicationContants;
import duypn.classes.HttpHelper;
import java.io.IOException;
import java.net.http.HttpResponse;
import java.util.HashMap;
import java.util.Map;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import org.json.JSONObject;

/**
 *
 * @author duypnse63523
 */
public class LoginController extends HttpServlet {

    /**
     * Processes requests for both HTTP <code>GET</code> and <code>POST</code>
     * methods.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html;charset=UTF-8");
        String url = ApplicationContants.ERROR;
        try {
            HttpSession session = request.getSession();
            String username = request.getParameter("email");
            String password = request.getParameter("password");
            String action = request.getParameter("action");
            
            HttpHelper h = new HttpHelper();
            
            if (session.getAttribute("Account") == null && username != null && password != null) {
                Map<Object, Object> data = new HashMap<>();
                
                
                data.put("username", username);
                data.put("password", password);
                
                
                HttpResponse<String> res = h.sendPost(data,ApplicationContants.URLPOSTLOGIN);

                JSONObject j = new JSONObject(res.body());
                
                if(j.getBoolean("Success")){
                    int role = j.getJSONObject("Data").getInt("roleId");
                    if (role == 2) {
                        url = ApplicationContants.ADMIN;
                        session.setAttribute("Account", j.getJSONObject("Data"));
                    } else {
                        request.setAttribute("ERROR", "Your role is not supported");
                    }
                }
                else {
                    request.setAttribute("ERROR", "Password or username is invalid");
                }
                
                 
            } else if(session.getAttribute("Account") != null){
                url = ApplicationContants.ADMIN;
            } else{
                url = ApplicationContants.LOGIN;
            }
        } catch (Exception e) {
            e.printStackTrace();
            log(e.getMessage());
        } finally {
            request.getRequestDispatcher(url).forward(request, response);
        }
    }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
