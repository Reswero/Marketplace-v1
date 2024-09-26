-- +goose Up
-- +goose StatementBegin
CREATE TABLE accounts (
    id Serial UNIQUE,
    type Varchar(20) NOT NULL,
    phone_number Varchar(30) NOT NULL,
    email Varchar(100) NOT NULL,
    created_at Timestamp NOT NULL,
    password Varchar(300) NOT NULL,
    salt Varchar(20) NOT NULL,
    PRIMARY KEY(type, phone_number)
);
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
DROP TABLE accounts;
-- +goose StatementEnd
