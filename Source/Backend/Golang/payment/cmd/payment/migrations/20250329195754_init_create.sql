-- +goose Up
-- +goose StatementBegin
CREATE TABLE pending_orders (
    order_id BIGINT PRIMARY KEY,
    payment_amount INTEGER NOT NULL,
    payment_id VARCHAR(20) NOT NULL,
    valid_until TIMESTAMP NOT NULL,
    UNIQUE(payment_url)
);

CREATE TABLE paid_orders (
    order_id BIGINT PRIMARY KEY,
    payment_amount INTEGER NOT NULL,
    paid_at TIMESTAMP NOT NULL
);
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
DROP TABLE pending_orders;
DROP TABLE paid_orders;
-- +goose StatementEnd
