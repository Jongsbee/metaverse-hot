package com.heosneverdie.A807PJT.common.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.springframework.http.HttpStatus;

@Builder
@Getter
@AllArgsConstructor
@NoArgsConstructor
public class ResponseDTO {
    private String msg;
    private String errMsg;
    private HttpStatus httpStatus;
    private Object data;
}

